using System;
using System.Collections.Generic;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Tanınması için sisteme eğitilen / eklenen bir kişiye ait profil ve yüz özellik vektörleri.
    /// </summary>
    public class PersonProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Her bir yüz örneğine ait normalize edilmiş öznitelik vektörleri (Grid Histograms)
        public List<float[]> FeatureVectors { get; } = new List<float[]>();

        public int SampleCount => FeatureVectors.Count;

        public override string ToString() => $"{Name} ({SampleCount} Yüz Örneği)";
    }
}
