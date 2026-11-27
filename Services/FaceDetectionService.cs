using System;
using System.Collections.Generic;
using System.IO;
using OpenCvSharp;
using FaceRecognition.Models;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Haar Cascade tabanlı nesne yönelimli yüz, göz (canlılık tespiti) ve gülümseme algılama servisi.
    /// Çoklu yüz tespiti, ölçeklendirme, liveness & anti-spoof ve görsel etiket çizimlerini yönetir.
    /// </summary>
    public class FaceDetectionService : IDisposable
    {
        private CascadeClassifier? _faceCascade;
        private CascadeClassifier? _eyeCascade;
        private CascadeClassifier? _smileCascade;
        private bool _disposed = false;

        public bool IsLoaded => _faceCascade != null && !_faceCascade.Empty();
        public bool IsEyeLoaded => _eyeCascade != null && !_eyeCascade.Empty();
        public bool IsSmileLoaded => _smileCascade != null && !_smileCascade.Empty();
        public string ModelPath { get; private set; } = string.Empty;

        // Algılama parametreleri
        public double ScaleFactor { get; set; } = 1.15;
        public int MinNeighbors { get; set; } = 4;
        public OpenCvSharp.Size MinFaceSize { get; set; } = new OpenCvSharp.Size(40, 40);
        public OpenCvSharp.Size MaxFaceSize { get; set; } = new OpenCvSharp.Size(600, 600);

        public event Action<string>? OnError;

        public FaceDetectionService(string? faceModelFileName = "haarcascade_frontalface_default.xml")
        {
            LoadModels(faceModelFileName);
        }

        public bool LoadModels(string? faceFileName)
        {
            bool okFace = LoadFaceModel(faceFileName ?? "haarcascade_frontalface_default.xml");
            LoadAuxiliaryModels();
            return okFace;
        }

        private bool LoadFaceModel(string fileName)
        {
            string path = FindModelPath(fileName);
            ModelPath = path;

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                OnError?.Invoke($"Haar Cascade yüz modeli bulunamadı: {fileName}\nLütfen model dosyasının uygulama dizininde olduğundan emin olunuz.");
                return false;
            }

            try
            {
                _faceCascade?.Dispose();
                _faceCascade = new CascadeClassifier(path);

                if (_faceCascade.Empty())
                {
                    OnError?.Invoke("Yüz model dosyası boş veya bozuk: " + path);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Yüz modeli yüklenirken hata oluştu: {ex.Message}");
                return false;
            }
        }

        private void LoadAuxiliaryModels()
        {
            try
            {
                // Göz modeli (Canlılık / Anti-spoof için)
                string eyePath = FindModelPath("haarcascade_eye.xml");
                if (File.Exists(eyePath))
                {
                    _eyeCascade?.Dispose();
                    _eyeCascade = new CascadeClassifier(eyePath);
                }

                // Gülümseme modeli (Mimik analizi için)
                string smilePath = FindModelPath("haarcascade_smile.xml");
                if (File.Exists(smilePath))
                {
                    _smileCascade?.Dispose();
                    _smileCascade = new CascadeClassifier(smilePath);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ek modeller (göz/gülümseme) yüklenirken uyarı: {ex.Message}");
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

                return _faceCascade!.DetectMultiScale(
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
        /// Yüz bölgesi üzerinde gözleri tespit eder (Canlılık analizi için).
        /// </summary>
        public Rect[] DetectEyes(Mat faceRegionGray)
        {
            if (!IsEyeLoaded || faceRegionGray == null || faceRegionGray.Empty()) return Array.Empty<Rect>();

            try
            {
                // Yüzün sadece üst yarısında göz ara (performans ve doğruluk için)
                int eyeAreaHeight = (int)(faceRegionGray.Height * 0.55);
                var eyeRoi = new Rect(0, 0, faceRegionGray.Width, eyeAreaHeight);
                using var eyeMat = new Mat(faceRegionGray, eyeRoi);

                return _eyeCascade!.DetectMultiScale(
                    image: eyeMat,
                    scaleFactor: 1.15,
                    minNeighbors: 3,
                    flags: HaarDetectionTypes.ScaleImage,
                    minSize: new OpenCvSharp.Size(12, 12),
                    maxSize: new OpenCvSharp.Size(faceRegionGray.Width / 2, faceRegionGray.Height / 2)
                );
            }
            catch
            {
                return Array.Empty<Rect>();
            }
        }

        /// <summary>
        /// Yüz bölgesi üzerinde gülümseme tespit eder (Mimik analizi için).
        /// </summary>
        public bool DetectSmile(Mat faceRegionGray)
        {
            if (!IsSmileLoaded || faceRegionGray == null || faceRegionGray.Empty()) return false;

            try
            {
                // Ağız alt yarıda yer alır
                int startY = (int)(faceRegionGray.Height * 0.45);
                int h = faceRegionGray.Height - startY;
                var mouthRoi = new Rect(faceRegionGray.Width / 6, startY, (faceRegionGray.Width * 2) / 3, h);
                using var mouthMat = new Mat(faceRegionGray, mouthRoi);

                var smiles = _smileCascade!.DetectMultiScale(
                    image: mouthMat,
                    scaleFactor: 1.35,
                    minNeighbors: 6,
                    flags: HaarDetectionTypes.ScaleImage,
                    minSize: new OpenCvSharp.Size(18, 18)
                );

                return smiles.Length > 0;
            }
            catch
            {
                return false;
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
        /// Görüntü üzerine algılanan yüzlerin dikdörtgen çerçevelerini, tanıma etiketlerini, canlılık ve mimik rozetlerini çizer.
        /// </summary>
        public void DrawFaceOverlays(Mat image, IReadOnlyList<DetectedFace> faces, bool showLiveness = true, bool showSmile = true)
        {
            if (image == null || image.Empty() || faces == null || faces.Count == 0) return;

            foreach (var face in faces)
            {
                Scalar boxColor = face.Recognition.IsRecognized 
                    ? new Scalar(34, 197, 94)   // Yeşil: Tanınan kişi
                    : new Scalar(239, 68, 68);  // Kırmızı: Bilinmeyen

                // 1. Dikdörtgen sınırlayıcı kutu
                Cv2.Rectangle(image, face.BoundingBox, boxColor, 2);

                // 2. Üst bilgi etiketi (İsim & Güven)
                string labelText = face.Recognition.IsRecognized
                    ? $"{face.Recognition.Label} ({face.Recognition.ConfidencePercentage})"
                    : "Bilinmeyen Kişi";

                var font = HersheyFonts.HersheySimplex;
                double fontScale = 0.52;
                int thickness = 1;
                var textSize = Cv2.GetTextSize(labelText, font, fontScale, thickness, out int baseline);

                int labelY = Math.Max(face.BoundingBox.Y - 6, textSize.Height + 6);
                OpenCvSharp.Point bgTopLeft = new OpenCvSharp.Point(face.BoundingBox.X, labelY - textSize.Height - 4);
                OpenCvSharp.Point bgBottomRight = new OpenCvSharp.Point(face.BoundingBox.X + textSize.Width + 8, labelY + baseline);

                Cv2.Rectangle(image, bgTopLeft, bgBottomRight, boxColor, -1);
                Cv2.PutText(image, labelText, new OpenCvSharp.Point(face.BoundingBox.X + 4, labelY), font, fontScale, Scalar.White, thickness, LineTypes.AntiAlias);

                // 3. Alt Rozetler (Canlılık & Gülümseme)
                int badgeY = face.BoundingBox.Y + face.BoundingBox.Height + 16;
                if (badgeY < image.Height - 8)
                {
                    int badgeX = face.BoundingBox.X;

                    if (showLiveness)
                    {
                        string liveTag = face.IsLive ? "CANLI [OK]" : "BEKLENIYOR...";
                        Scalar liveCol = face.IsLive ? new Scalar(34, 197, 94) : new Scalar(234, 179, 8);
                        Cv2.PutText(image, liveTag, new OpenCvSharp.Point(badgeX, badgeY), font, 0.42, liveCol, 1, LineTypes.AntiAlias);
                        badgeX += 105;
                    }

                    if (showSmile && face.IsSmiling)
                    {
                        Cv2.PutText(image, "GULUMSEME :)", new OpenCvSharp.Point(badgeX, badgeY), font, 0.42, new Scalar(251, 146, 60), 1, LineTypes.AntiAlias);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _faceCascade?.Dispose();
                _eyeCascade?.Dispose();
                _smileCascade?.Dispose();
            }
        }
    }
}
