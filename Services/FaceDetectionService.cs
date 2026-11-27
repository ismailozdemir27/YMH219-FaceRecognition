using System;
using System.Collections.Generic;
using System.IO;
using OpenCvSharp;
using FaceRecognition.Models;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Haar Cascade tabanlı nesne yönelimli yüz algılama servisi.
    /// Çoklu yüz tespiti, ölçeklendirme ve görsel kutu çizimlerini yönetir.
    /// </summary>
    public class FaceDetectionService : IDisposable
    {
        private CascadeClassifier? _cascade;
        private bool _disposed = false;

        public bool IsLoaded => _cascade != null && !_cascade.Empty();
        public string ModelPath { get; private set; } = string.Empty;

        // Algılama parametreleri
        public double ScaleFactor { get; set; } = 1.15;
        public int MinNeighbors { get; set; } = 4;
        public OpenCvSharp.Size MinFaceSize { get; set; } = new OpenCvSharp.Size(40, 40);
        public OpenCvSharp.Size MaxFaceSize { get; set; } = new OpenCvSharp.Size(600, 600);

        public event Action<string>? OnError;

        public FaceDetectionService(string? modelFileName = "haarcascade_frontalface_default.xml")
        {
            LoadModel(modelFileName);
        }

        public bool LoadModel(string? fileName)
        {
            string path = FindModelPath(fileName ?? "haarcascade_frontalface_default.xml");
            ModelPath = path;

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                OnError?.Invoke($"Haar Cascade model dosyası bulunamadı: {fileName}\nLütfen model dosyasının uygulama dizininde olduğundan emin olunuz.");
                return false;
            }

            try
            {
                _cascade?.Dispose();
                _cascade = new CascadeClassifier(path);

                if (_cascade.Empty())
                {
                    OnError?.Invoke("Model dosyası boş veya bozuk: " + path);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Model yüklenirken hata oluştu: {ex.Message}");
                return false;
            }
        }

        private static string FindModelPath(string fileName)
        {
            string sourcePath = string.Empty;
            string direct = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(direct)) sourcePath = direct;
            else
            {
                string parent = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", fileName);
                if (File.Exists(parent)) sourcePath = Path.GetFullPath(parent);
                else if (File.Exists(fileName)) sourcePath = Path.GetFullPath(fileName);
            }

            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                return direct;

            // OpenCV C++ kütüphanesinin Türkçe / Unicode karakterli yollarda çökmesini engellemek için
            // modeli güvenli ve salt-ASCII ProgramData dizinine kopyalarız:
            try
            {
                string programDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FaceRecognition");
                Directory.CreateDirectory(programDataDir);
                string asciiSafeTarget = Path.Combine(programDataDir, fileName);
                if (!File.Exists(asciiSafeTarget) || new FileInfo(asciiSafeTarget).Length != new FileInfo(sourcePath).Length)
                {
                    File.Copy(sourcePath, asciiSafeTarget, true);
                }
                return asciiSafeTarget;
            }
            catch
            {
                return GetShortPath(sourcePath);
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int GetShortPathName(string lpszLongPath, System.Text.StringBuilder lpszShortPath, int cchBuffer);

        private static string GetShortPath(string path)
        {
            try
            {
                var sb = new System.Text.StringBuilder(1024);
                int len = GetShortPathName(path, sb, sb.Capacity);
                if (len > 0) return sb.ToString();
            }
            catch { }
            return path;
        }

        /// <summary>
        /// Verilen görüntü üzerindeki tüm yüzleri tespit eder.
        /// </summary>
        public Rect[] DetectFaces(Mat image)
        {
            if (!IsLoaded || image == null || image.Empty()) return Array.Empty<Rect>();

            try
            {
                using var gray = new Mat();
                if (image.Channels() == 3 || image.Channels() == 4)
                {
                    Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                }
                else
                {
                    image.CopyTo(gray);
                }

                Cv2.EqualizeHist(gray, gray);

                return _cascade!.DetectMultiScale(
                    image: gray,
                    scaleFactor: ScaleFactor,
                    minNeighbors: MinNeighbors,
                    flags: HaarDetectionTypes.ScaleImage,
                    minSize: MinFaceSize,
                    maxSize: MaxFaceSize
                );
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Yüz algılama hatası: {ex.Message}");
                return Array.Empty<Rect>();
            }
        }

        /// <summary>
        /// Kare içindeki yüz bölgesini normalize ederek (100x100 gri ölçekli ve histogramı eşitlenmiş) döndürür.
        /// </summary>
        public Mat? ExtractNormalizedFace(Mat originalImage, Rect faceBox)
        {
            if (originalImage == null || originalImage.Empty()) return null;

            try
            {
                // Kutunun görüntü sınırları içinde kalmasını sağla
                Rect safeBox = new Rect(
                    Math.Max(0, faceBox.X),
                    Math.Max(0, faceBox.Y),
                    Math.Min(faceBox.Width, originalImage.Width - Math.Max(0, faceBox.X)),
                    Math.Min(faceBox.Height, originalImage.Height - Math.Max(0, faceBox.Y))
                );

                if (safeBox.Width <= 10 || safeBox.Height <= 10) return null;

                using var cropped = new Mat(originalImage, safeBox);
                using var gray = new Mat();

                if (cropped.Channels() >= 3)
                    Cv2.CvtColor(cropped, gray, ColorConversionCodes.BGR2GRAY);
                else
                    cropped.CopyTo(gray);

                var normalized = new Mat();
                Cv2.Resize(gray, normalized, new OpenCvSharp.Size(100, 100));
                Cv2.EqualizeHist(normalized, normalized);

                return normalized;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Görüntü üzerine algılanan yüzlerin dikdörtgen çerçevelerini ve tanıma etiketlerini çizer.
        /// </summary>
        public void DrawFaceOverlays(Mat image, IReadOnlyList<DetectedFace> faces)
        {
            if (image == null || image.Empty() || faces == null || faces.Count == 0) return;

            foreach (var face in faces)
            {
                Scalar boxColor = face.Recognition.IsRecognized 
                    ? new Scalar(34, 197, 94)   // Yeşil: Tanınan kişi
                    : new Scalar(239, 68, 68);  // Kırmızı: Bilinmeyen

                // 1. Köşe stilli şık dikdörtgen çerçeve
                Cv2.Rectangle(image, face.BoundingBox, boxColor, 2);

                // 2. Üst bilgi etiketi (İsim & Güven)
                string labelText = face.Recognition.IsRecognized
                    ? $"{face.Recognition.Label} ({face.Recognition.ConfidencePercentage})"
                    : "Bilinmeyen Kişi";

                var font = HersheyFonts.HersheySimplex;
                double fontScale = 0.55;
                int thickness = 1;
                var textSize = Cv2.GetTextSize(labelText, font, fontScale, thickness, out int baseline);

                int labelY = Math.Max(face.BoundingBox.Y - 6, textSize.Height + 6);
                OpenCvSharp.Point bgTopLeft = new OpenCvSharp.Point(face.BoundingBox.X, labelY - textSize.Height - 4);
                OpenCvSharp.Point bgBottomRight = new OpenCvSharp.Point(face.BoundingBox.X + textSize.Width + 8, labelY + baseline);

                // Etiket arka planı
                Cv2.Rectangle(image, bgTopLeft, bgBottomRight, boxColor, -1);
                // Etiket metni
                Cv2.PutText(image, labelText, new OpenCvSharp.Point(face.BoundingBox.X + 4, labelY), font, fontScale, Scalar.White, thickness, LineTypes.AntiAlias);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _cascade?.Dispose();
                _cascade = null;
            }
        }
    }
}
