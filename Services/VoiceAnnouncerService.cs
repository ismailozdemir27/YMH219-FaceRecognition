using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Tanınan veya bilinmeyen kişiler için Türkçe sesli karşılama, anons ve uyarı
    /// üreten asenkron ses sentezleme servisi (Text-to-Speech).
    /// </summary>
    public class VoiceAnnouncerService : IDisposable
    {
        private SpeechSynthesizer? _synthesizer;
        private readonly ConcurrentDictionary<string, DateTime> _lastAnnouncementTimes = new();
        private readonly TimeSpan _cooldown = TimeSpan.FromSeconds(8); // Aynı kişi için tekrar bekleme süresi
        private bool _disposed = false;

        public bool IsEnabled { get; set; } = true;

        public VoiceAnnouncerService()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SetOutputToDefaultAudioDevice();

                // Türkçe ses seçimi (Tolga veya sistemdeki ilk Türkçe ses)
                foreach (var voice in _synthesizer.GetInstalledVoices())
                {
                    var info = voice?.VoiceInfo;
                    if (info != null && !string.IsNullOrEmpty(info.Name))
                    {
                        bool isTr = info.Culture != null && info.Culture.TwoLetterISOLanguageName.Equals("tr", StringComparison.OrdinalIgnoreCase);
                        bool hasName = info.Name.Contains("Tolga", StringComparison.OrdinalIgnoreCase) || info.Name.Contains("Turkish", StringComparison.OrdinalIgnoreCase);
                        if (isTr || hasName)
                        {
                            _synthesizer.SelectVoice(info.Name);
                            break;
                        }
                    }
                }
            }
            catch
            {
                _synthesizer = null;
            }
        }

        /// <summary>
        /// Tanınan bir kişi için isimle sesli karşılama yapar.
        /// </summary>
        public void AnnounceRecognized(string personName)
        {
            if (!IsEnabled || _synthesizer == null || string.IsNullOrWhiteSpace(personName)) return;

            DateTime now = DateTime.Now;
            if (_lastAnnouncementTimes.TryGetValue(personName, out var lastTime) && (now - lastTime) < _cooldown)
            {
                return; // Bekleme süresi henüz dolmadı
            }

            _lastAnnouncementTimes[personName] = now;

            string message = personName.Contains("İsmail", StringComparison.OrdinalIgnoreCase)
                ? "Hoş geldiniz İsmail Bey. Sistem sizi tanıdı."
                : $"Hoş geldiniz {personName}.";

            SpeakAsync(message);
        }

        /// <summary>
        /// Bilinmeyen bir kişi algılandığında güvenlik uyarısı yapar.
        /// </summary>
        public void AnnounceUnknown()
        {
            if (!IsEnabled || _synthesizer == null) return;

            string key = "__UNKNOWN__";
            DateTime now = DateTime.Now;
            if (_lastAnnouncementTimes.TryGetValue(key, out var lastTime) && (now - lastTime) < _cooldown)
            {
                return;
            }

            _lastAnnouncementTimes[key] = now;
            SpeakAsync("Dikkat: Tanımlanamayan kişi algılandı.");
        }

        private void SpeakAsync(string text)
        {
            Task.Run(() =>
            {
                try
                {
                    _synthesizer?.SpeakAsyncCancelAll();
                    _synthesizer?.Speak(text);
                }
                catch { }
            });
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _synthesizer?.Dispose();
            }
        }
    }
}
