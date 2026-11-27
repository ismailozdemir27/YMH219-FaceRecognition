using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using FaceRecognition.Enums;

namespace FaceRecognition.Services
{
    /// <summary>
    /// DirectShow öncelikli, Media Foundation yedekli ve gerektiğinde yerleşik sanal kamera
    /// simülasyonu sunan gelişmiş, nesne yönelimli kamera servisi.
    /// </summary>
    public class CameraService : IDisposable
    {
        private VideoCapture? _capture;
        private CancellationTokenSource? _cts;
        private CameraStatus _status = CameraStatus.Stopped;
        private bool _disposed = false;

        public CameraStatus Status
        {
            get => _status;
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusChanged?.Invoke(_status, GetStatusMessage(_status));
                }
            }
        }

        public bool IsRunning => Status == CameraStatus.Running;
        public int CameraIndex { get; private set; } = 0;
        public bool IsVirtualCamera => CameraIndex == -1;
        public double CurrentFps { get; private set; } = 0.0;

        public event Action<Mat, double>? OnFrameCaptured; // frame, fps (Subscriber owns Mat)
        public event Action<CameraStatus, string>? OnStatusChanged;
        public event Action<string>? OnError;

        public bool Start(int cameraIndex = 0)
        {
            if (IsRunning) return true;

            try
            {
                CameraIndex = cameraIndex;
                _cts = new CancellationTokenSource();

                // 1. Sanal Test Kamerası (Simülasyon Modu)
                if (cameraIndex == -1)
                {
                    Status = CameraStatus.Running;
                    _ = Task.Run(() => VirtualCameraLoopAsync(_cts.Token));
                    return true;
                }

                // 2. Fiziksel Kamera Başlatma (DirectShow öncelikli)
                _capture = TryOpenPhysicalCamera(cameraIndex);

                if (_capture == null || !_capture.IsOpened())
                {
                    Status = CameraStatus.Error;
                    OnError?.Invoke($"Kamera (Aygıt #{cameraIndex}) açılamadı!\n\n" +
                                   "Olası Nedenler:\n" +
                                   "1. Kameranız başka bir uygulama (Zoom, Teams, Tarayıcı) tarafından kullanılıyor olabilir.\n" +
                                   "2. Windows Gizlilik Ayarları'nda masaüstü uygulamalarının kamera erişimi kapalı olabilir.\n\n" +
                                   "İpucu: Kamera seçim kutusundan '🎮 Sanal Kamera (Test Simülatörü)' modunu seçerek " +
                                   "yüz algılama, sesli karşılama ve alarm özelliklerini anında test edebilirsiniz!");
                    _capture?.Dispose();
                    _capture = null;
                    return false;
                }

                // Çözünürlük ayarını güvenli şekilde dene (Hata verirse kameranın varsayılan çözünürlüğü korunur)
                try
                {
                    _capture.Set(VideoCaptureProperties.FrameWidth, 640);
                    _capture.Set(VideoCaptureProperties.FrameHeight, 480);
                }
                catch { }

                Status = CameraStatus.Running;
                _ = Task.Run(() => CaptureLoopAsync(_cts.Token));
                return true;
            }
            catch (Exception ex)
            {
                Status = CameraStatus.Error;
                OnError?.Invoke($"Kamera başlatılırken hata oluştu: {ex.Message}");
                return false;
            }
        }

        private VideoCapture? TryOpenPhysicalCamera(int cameraIndex)
        {
            VideoCapture? cap = null;

            // Öncelik 1: DirectShow (Windows web kameraları için en uyumlu ve kararlı motor)
            try
            {
                cap = new VideoCapture(cameraIndex, VideoCaptureAPIs.DSHOW);
                if (cap.IsOpened()) return cap;
                cap.Dispose();
            }
            catch { cap?.Dispose(); }

            // Öncelik 2: Media Foundation (MSMF)
            try
            {
                cap = new VideoCapture(cameraIndex, VideoCaptureAPIs.MSMF);
                if (cap.IsOpened()) return cap;
                cap.Dispose();
            }
            catch { cap?.Dispose(); }

            // Öncelik 3: Otomatik (ANY)
            try
            {
                cap = new VideoCapture(cameraIndex, VideoCaptureAPIs.ANY);
                if (cap.IsOpened()) return cap;
                cap.Dispose();
            }
            catch { cap?.Dispose(); }

            return null;
        }

        private async Task CaptureLoopAsync(CancellationToken token)
        {
            var sw = Stopwatch.StartNew();
            int frameCount = 0;
            double fpsTimer = 0.0;

            using var frame = new Mat();

            while (!token.IsCancellationRequested && _capture != null && _capture.IsOpened())
            {
                try
                {
                    if (!_capture.Read(frame) || frame.Empty())
                    {
                        await Task.Delay(15, token);
                        continue;
                    }

                    // FPS hesabı
                    frameCount++;
                    double elapsed = sw.Elapsed.TotalSeconds;
                    if (elapsed - fpsTimer >= 0.5)
                    {
                        CurrentFps = Math.Round(frameCount / (elapsed - fpsTimer), 1);
                        frameCount = 0;
                        fpsTimer = elapsed;
                    }

                    // Klonlayarak alıcıya teslim et (Alıcı Dispose sorumluluğunu üstlenir)
                    var clone = frame.Clone();
                    OnFrameCaptured?.Invoke(clone, CurrentFps);

                    // ~30 FPS hedefi için ufak gecikme
                    await Task.Delay(25, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        OnError?.Invoke($"Kare okuma hatası: {ex.Message}");
                    }
                    break;
                }
            }

            Status = CameraStatus.Stopped;
        }

        /// <summary>
        /// Fiziksel kamerası olmayan veya meşgul olan sistemler için dinamik hareketli yüz simülasyonu.
        /// </summary>
        private async Task VirtualCameraLoopAsync(CancellationToken token)
        {
            var sw = Stopwatch.StartNew();
            int frameCount = 0;
            double fpsTimer = 0.0;
            double angle = 0.0;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    angle += 0.08;
                    int offsetX = (int)(Math.Sin(angle) * 45);
                    int offsetY = (int)(Math.Cos(angle * 0.7) * 25);
                    bool blink = ((int)(angle * 2) % 14) == 0; // Periyodik göz kırpma

                    using var simMat = new Mat(480, 640, MatType.CV_8UC3, new Scalar(25, 33, 44));

                    // Arka plan kılavuz çizgileri
                    for (int x = 0; x < 640; x += 80)
                        Cv2.Line(simMat, new OpenCvSharp.Point(x, 0), new OpenCvSharp.Point(x, 480), new Scalar(35, 45, 60), 1);
                    for (int y = 0; y < 480; y += 80)
                        Cv2.Line(simMat, new OpenCvSharp.Point(0, y), new OpenCvSharp.Point(640, y), new Scalar(35, 45, 60), 1);

                    // Simüle Yüz (İsmail Özdemir - Canlı hareketli yüz)
                    int cx = 320 + offsetX;
                    int cy = 230 + offsetY;

                    // Yüz gövdesi ve kafa
                    Cv2.Ellipse(simMat, new OpenCvSharp.Point(cx, cy), new OpenCvSharp.Size(85, 110), 0, 0, 360, new Scalar(210, 180, 140), -1);
                    Cv2.Ellipse(simMat, new OpenCvSharp.Point(cx, cy - 80), new OpenCvSharp.Size(85, 35), 0, 180, 360, new Scalar(35, 25, 20), -1); // Saç

                    // Gözler
                    int eyeY = cy - 25;
                    if (blink)
                    {
                        // Kapalı gözler (Kırpma)
                        Cv2.Line(simMat, new OpenCvSharp.Point(cx - 35, eyeY), new OpenCvSharp.Point(cx - 15, eyeY), new Scalar(20, 20, 20), 2);
                        Cv2.Line(simMat, new OpenCvSharp.Point(cx + 15, eyeY), new OpenCvSharp.Point(cx + 35, eyeY), new Scalar(20, 20, 20), 2);
                    }
                    else
                    {
                        // Açık gözler
                        Cv2.Circle(simMat, new OpenCvSharp.Point(cx - 25, eyeY), 10, new Scalar(240, 240, 240), -1);
                        Cv2.Circle(simMat, new OpenCvSharp.Point(cx + 25, eyeY), 10, new Scalar(240, 240, 240), -1);
                        Cv2.Circle(simMat, new OpenCvSharp.Point(cx - 25, eyeY), 5, new Scalar(40, 40, 40), -1);
                        Cv2.Circle(simMat, new OpenCvSharp.Point(cx + 25, eyeY), 5, new Scalar(40, 40, 40), -1);
                    }

                    // Burun & Gülümseyen Ağız
                    Cv2.Line(simMat, new OpenCvSharp.Point(cx, cy - 10), new OpenCvSharp.Point(cx, cy + 15), new Scalar(180, 150, 115), 3);
                    Cv2.Ellipse(simMat, new OpenCvSharp.Point(cx, cy + 35), new OpenCvSharp.Size(28, 14), 0, 0, 180, new Scalar(40, 40, 190), 3);

                    // Filigran
                    Cv2.PutText(simMat, "[SANAL TEST KAMERASI - CANLI SIMULASYON]", new OpenCvSharp.Point(16, 30), HersheyFonts.HersheySimplex, 0.55, new Scalar(74, 222, 128), 1);

                    // FPS
                    frameCount++;
                    double elapsed = sw.Elapsed.TotalSeconds;
                    if (elapsed - fpsTimer >= 0.5)
                    {
                        CurrentFps = Math.Round(frameCount / (elapsed - fpsTimer), 1);
                        frameCount = 0;
                        fpsTimer = elapsed;
                    }

                    var clone = simMat.Clone();
                    OnFrameCaptured?.Invoke(clone, CurrentFps);

                    await Task.Delay(33, token); // ~30 FPS
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    break;
                }
            }

            Status = CameraStatus.Stopped;
        }

        public void Stop()
        {
            if (!IsRunning && _capture == null) return;

            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;

                if (_capture != null)
                {
                    _capture.Release();
                    _capture.Dispose();
                    _capture = null;
                }

                CurrentFps = 0.0;
                Status = CameraStatus.Stopped;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Kamera durdurulurken hata: {ex.Message}");
            }
        }

        private static string GetStatusMessage(CameraStatus status) => status switch
        {
            CameraStatus.Running => "● Kamera Canlı Yayında",
            CameraStatus.Stopped => "■ Kamera Durduruldu",
            CameraStatus.Error => "⚠ Kamera Açılamadı",
            _ => "Bilinmiyor"
        };

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Stop();
            }
        }
    }
}
