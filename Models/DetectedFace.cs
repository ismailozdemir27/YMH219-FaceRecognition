using System;
using System.Drawing;
using OpenCvSharp;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Görüntü üzerinde algılanan tekil bir yüzü ve tanıma sonucunu temsil eden model.
    /// </summary>
    public class DetectedFace
    {
        public int Id { get; set; }
        public Rect BoundingBox { get; set; }
        public RecognitionResult Recognition { get; set; } = RecognitionResult.Unknown();
        public Bitmap? CropBitmap { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Gelişmiş öznitelikler (Canlılık, Göz ve Gülümseme)
        public bool IsLive { get; set; } = false;
        public int EyesCount { get; set; } = 0;
        public bool IsSmiling { get; set; } = false;

        public string FormattedTimestamp => Timestamp.ToString("HH:mm:ss.fff");

        public string DisplaySummary => $"{Recognition.Label} - Güven: {Recognition.ConfidencePercentage} - Canlı: {(IsLive ? "Evet" : "Bilinmiyor")} - Gülümseme: {(IsSmiling ? "Var" : "Yok")}";
    }
}
