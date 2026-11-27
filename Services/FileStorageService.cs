using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using FaceRecognition.Models;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Kırpılan yüzlerin diske kaydedilmesi, rapor çıktısı alınması ve
    /// dosya sistemi işlemlerini yöneten servis.
    /// </summary>
    public class FileStorageService
    {
        public string CroppedFacesDirectory { get; }

        public FileStorageService()
        {
            CroppedFacesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CroppedFaces");
            if (!Directory.Exists(CroppedFacesDirectory))
            {
                Directory.CreateDirectory(CroppedFacesDirectory);
            }
        }

        /// <summary>
        /// Kılavuzdaki isteğe bağlı ek puan gereksinimi: Algılanan yüzü kırparak ayrı klasöre kaydeder.
        /// </summary>
        public string? SaveCroppedFace(Bitmap faceBitmap, string personName, float confidence)
        {
            if (faceBitmap == null) return null;

            try
            {
                string safeName = string.Join("_", personName.Split(Path.GetInvalidFileNameChars()));
                string timeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                int confInt = (int)Math.Round(confidence * 100);

                string fileName = $"Face_{timeStr}_{safeName}_{confInt}pct.png";
                string fullPath = Path.Combine(CroppedFacesDirectory, fileName);

                faceBitmap.Save(fullPath, ImageFormat.Png);
                return fullPath;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Yüz tanıma olaylarını StreamWriter kullanarak detaylı UTF-8 raporu olarak kaydeder.
        /// </summary>
        public bool ExportDetectionHistoryReport(string filePath, IReadOnlyList<DetectedFace> faces)
        {
            return SaveRecognitionLogToFile(filePath, faces);
        }

        public bool SaveRecognitionLogToFile(string filePath, IReadOnlyList<DetectedFace> faces, string studentInfo = "İsmail Özdemir - 250541109")
        {
            try
            {
                using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
                writer.WriteLine("================================================================================");
                writer.WriteLine("           YMH219 - OPENCV YÜZ ALGILAMA VE TANIMA GÜNLÜK RAPORU                ");
                writer.WriteLine("================================================================================");
                writer.WriteLine($"Rapor Tarihi       : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Öğrenci Bilgisi    : {studentInfo}");
                writer.WriteLine($"Tespit Edilen Yüz  : {faces.Count}");
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine(string.Format("{0,-5} | {1,-12} | {2,-20} | {3,-10} | {4}", "NO", "ZAMAN", "KİŞİ ETİKETİ", "GÜVEN", "YÜZ ALANI (X, Y, W, H)"));
                writer.WriteLine("--------------------------------------------------------------------------------");

                int index = 1;
                foreach (var face in faces)
                {
                    string boxStr = $"[{face.BoundingBox.X}, {face.BoundingBox.Y}, {face.BoundingBox.Width}x{face.BoundingBox.Height}]";
                    writer.WriteLine(string.Format("{0,-5} | {1,-12} | {2,-20} | {3,-10} | {4}",
                        index++,
                        face.FormattedTimestamp,
                        face.Recognition.Label,
                        face.Recognition.ConfidencePercentage,
                        boxStr));
                }

                writer.WriteLine("================================================================================");
                writer.WriteLine("RAPOR SONU - Nesne Tabanlı Programlama (YMH 219)");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
