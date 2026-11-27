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

        public string FormattedTimestamp => Timestamp.ToString("HH:mm:ss.fff");

        public string DisplaySummary => $"{Recognition.Label} - Güven: {Recognition.ConfidencePercentage} - Alan: [{BoundingBox.Width}x{BoundingBox.Height}]";
    }
}
