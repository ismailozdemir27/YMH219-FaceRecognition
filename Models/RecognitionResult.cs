using System;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Yüz tanıma modelinin tahmin ve eşleşme sonucunu tutan sınıf.
    /// </summary>
    public class RecognitionResult
    {
        public string Label { get; set; } = "Bilinmeyen";
        public float Confidence { get; set; } = 0.0f; // 0.0 ile 1.0 arası güven oranı
        public double Distance { get; set; } = 1.0;
        public bool IsRecognized { get; set; } = false;

        public string ConfidencePercentage => $"{Math.Round(Confidence * 100, 1)}%";

        public static RecognitionResult Unknown(double distance = 1.0) => new RecognitionResult
        {
            Label = "Bilinmeyen",
            Confidence = 0.0f,
            Distance = distance,
            IsRecognized = false
        };

        public static RecognitionResult Recognized(string name, float confidence, double distance) => new RecognitionResult
        {
            Label = name,
            Confidence = confidence,
            Distance = distance,
            IsRecognized = true
        };

        public override string ToString() => IsRecognized ? $"{Label} ({ConfidencePercentage})" : "Bilinmeyen";
    }
}
