namespace FaceRecognition.Enums
{
    /// <summary>
    /// Görüntü kaynağı türleri.
    /// </summary>
    public enum CaptureSource
    {
        None,           // Kaynak yok / beklemede
        Camera,         // Canlı Webcam / USB Kamera
        StaticImage     // Dosyadan yüklenen statik resim
    }
}
