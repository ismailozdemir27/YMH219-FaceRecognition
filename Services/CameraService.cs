using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using FaceRecognition.Enums;

namespace FaceRecognition.Services
{
    /// <summary>
    /// VideoCapture tabanlı kamera donanımını yöneten, arka planda güvenli kare döngüsü ve
    /// canlı FPS hesaplaması sunan kamera servisi.
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
        public double CurrentFps { get; private set; } = 0.0;

        public event Action<Mat, double>? OnFrameCaptured; // frame, fps
        public event Action<CameraStatus, string>? OnStatusChanged;
        public event Action<string>? OnError;

        public bool Start(int cameraIndex = 0)
        {
            if (IsRunning) return true;

            try
            {
                CameraIndex = cameraIndex;
                _capture = new VideoCapture(cameraIndex, VideoCaptureAPIs.ANY);

                if (!_capture.IsOpened())
                {
                    Status = CameraStatus.Error;
                    OnError?.Invoke($"Kamera (Aygıt #{cameraIndex}) açılamadı!\n" +
                                   "Lütfen web kameranızın takılı ve izinlerinin açık olduğundan emin olunuz " +
                                   "veya 'Görüntü Seç' butonu ile resim üzerinden test yapınız.");
                    _capture.Dispose();
                    _capture = null;
                    return false;
                }

                // İdeal çözünürlük ayarla (640x480 akıcı işlem için)
                _capture.Set(VideoCaptureProperties.FrameWidth, 640);
                _capture.Set(VideoCaptureProperties.FrameHeight, 480);

                _cts = new CancellationTokenSource();
                Status = CameraStatus.Running;

                // Arka planda kare okuma döngüsünü başlat
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

                    // Klonlayarak UI thread'e veya işleyiciye teslim et
                    using var clone = frame.Clone();
                    OnFrameCaptured?.Invoke(clone, CurrentFps);

                    // ~30 FPS hedefi için ufak gecikme
                    await Task.Delay(20, token);
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
