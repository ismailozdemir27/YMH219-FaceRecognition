using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Bilinmeyen veya şüpheli kişilerin sürekli kamerada kalması durumunda
    /// sesli alarm, görsel kırmızı flaş ve anlık delil fotoğrafı kaydeden güvenlik alarm servisi.
    /// </summary>
    public class SecurityAlarmService
    {
        private DateTime _unknownStartTime = DateTime.MinValue;
        private DateTime _lastAlarmSoundTime = DateTime.MinValue;
        private readonly string _alertsDirectory;

        public bool IsEnabled { get; set; } = false;
        public int TriggerSeconds { get; set; } = 3; // 3 saniye kesintisiz bilinmeyen kişi
        public bool IsAlarmActive { get; private set; } = false;

        public event Action<string>? OnAlarmTriggered;
        public event Action? OnAlarmCleared;

        public SecurityAlarmService()
        {
            _alertsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Alerts");
            Directory.CreateDirectory(_alertsDirectory);
        }

        /// <summary>
        /// Her karede çağrılarak şüpheli durumu değerlendirir.
        /// </summary>
        public void EvaluateFrame(bool hasUnknownFace, Bitmap? currentFrameBitmap)
        {
            if (!IsEnabled)
            {
                if (IsAlarmActive)
                {
                    IsAlarmActive = false;
                    OnAlarmCleared?.Invoke();
                }
                _unknownStartTime = DateTime.MinValue;
                return;
            }

            if (hasUnknownFace)
            {
                if (_unknownStartTime == DateTime.MinValue)
                {
                    _unknownStartTime = DateTime.Now;
                }
                else
                {
                    double elapsed = (DateTime.Now - _unknownStartTime).TotalSeconds;
                    if (elapsed >= TriggerSeconds)
                    {
                        if (!IsAlarmActive)
                        {
                            IsAlarmActive = true;
                            SaveAlertSnapshot(currentFrameBitmap);
                        }

                        // Periyodik sesli ikaz
                        if ((DateTime.Now - _lastAlarmSoundTime).TotalSeconds >= 1.5)
                        {
                            SystemSounds.Exclamation.Play();
                            _lastAlarmSoundTime = DateTime.Now;
                            OnAlarmTriggered?.Invoke($"🚨 GÜVENLİK ALARMI: {elapsed:F0} saniyedir yetkisiz/bilinmeyen kişi tespit ediliyor!");
                        }
                    }
                }
            }
            else
            {
                if (IsAlarmActive)
                {
                    IsAlarmActive = false;
                    OnAlarmCleared?.Invoke();
                }
                _unknownStartTime = DateTime.MinValue;
            }
        }

        private void SaveAlertSnapshot(Bitmap? bmp)
        {
            if (bmp == null) return;

            try
            {
                string filename = $"Alarm_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string fullPath = Path.Combine(_alertsDirectory, filename);
                bmp.Save(fullPath, ImageFormat.Png);
            }
            catch { }
        }

        public void Reset()
        {
            _unknownStartTime = DateTime.MinValue;
            IsAlarmActive = false;
            OnAlarmCleared?.Invoke();
        }
    }
}
