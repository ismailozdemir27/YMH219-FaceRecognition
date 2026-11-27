using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using FaceRecognition.Enums;
using FaceRecognition.Models;
using FaceRecognition.Services;

namespace FaceRecognition.Forms
{
    public partial class MainForm : Form
    {
        private readonly FaceDetectionService _detectionService;
        private readonly FaceRecognitionService _recognitionService;
        private readonly CameraService _cameraService;
        private readonly FileStorageService _storageService;
        private readonly VoiceAnnouncerService _voiceService;
        private readonly SecurityAlarmService _alarmService;

        private Mat? _lastRawFrame;
        private readonly List<DetectedFace> _detectionHistory = new List<DetectedFace>();
        private int _detectionCounter = 0;
        private DateTime _lastAutoSaveTime = DateTime.MinValue;
        private bool _isDarkTheme = true;

        public MainForm()
        {
            InitializeComponent();

            _detectionService = new FaceDetectionService();
            _recognitionService = new FaceRecognitionService();
            _cameraService = new CameraService();
            _storageService = new FileStorageService();
            _voiceService = new VoiceAnnouncerService();
            _alarmService = new SecurityAlarmService();

            InitializeCameraDropdown();
            InitializeDetectionsGrid();
            RefreshProfilesList();
            BindEvents();

            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            UpdateCameraStatusUI(CameraStatus.Stopped, "Kamera Kapalı");
            UpdateSensitivityLabels();
            ApplyTheme(_isDarkTheme);

            if (!_detectionService.IsLoaded)
            {
                lblFooterStatus.Text = "⚠ Haar Cascade yüz modeli yüklenemedi!";
                lblFooterStatus.ForeColor = Color.Red;
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _cameraService.Stop();
            _cameraService.Dispose();
            _detectionService.Dispose();
            _voiceService.Dispose();
            _lastRawFrame?.Dispose();
            pbVideoFeed.Image?.Dispose();
        }

        #region --- BAŞLANGIÇ AYARLARI VE LİSTELER ---

        private void InitializeCameraDropdown()
        {
            cmbCameraDevice.Items.Clear();
            cmbCameraDevice.Items.Add(new CameraDeviceItem(0, "Kamera #0 (Dahili HD Webcam)"));
            cmbCameraDevice.Items.Add(new CameraDeviceItem(1, "Kamera #1 (Harici USB Kamera)"));
            cmbCameraDevice.Items.Add(new CameraDeviceItem(-1, "🎮 Sanal Kamera (Test Simülatörü)"));
            cmbCameraDevice.SelectedIndex = 0;
        }

        private void InitializeDetectionsGrid()
        {
            dgvDetections.Columns.Clear();

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "#",
                Width = 35
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTime",
                HeaderText = "Zaman",
                Width = 70
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Kişi / Etiket",
                Width = 115
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colConfidence",
                HeaderText = "Güven",
                Width = 65
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colLive",
                HeaderText = "Canlı",
                Width = 50
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSmile",
                HeaderText = "Mimik",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void RefreshProfilesList()
        {
            lstProfiles.Items.Clear();
            foreach (var profile in _recognitionService.Profiles)
            {
                lstProfiles.Items.Add(profile);
            }
            grpProfiles.Text = $"👤 Eğitilmiş Gönüllü Yüz Profilleri ({_recognitionService.Profiles.Count})";
        }

        private void UpdateSensitivityLabels()
        {
            double scale = 1.0 + (tbScaleFactor.Value * 0.015);
            _detectionService.ScaleFactor = Math.Round(scale, 2);
            lblScaleFactorVal.Text = _detectionService.ScaleFactor.ToString("0.00");

            _detectionService.MinNeighbors = tbNeighbors.Value;
            lblNeighborsVal.Text = tbNeighbors.Value.ToString();
        }

        #endregion

        #region --- OLAY YÖNETİMİ ---

        private void BindEvents()
        {
            _cameraService.OnFrameCaptured += Camera_OnFrameCaptured;
            _cameraService.OnStatusChanged += Camera_OnStatusChanged;
            _cameraService.OnError += Camera_OnError;
            _detectionService.OnError += Detection_OnError;

            _alarmService.OnAlarmTriggered += msg =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => lblFooterStatus.Text = msg));
                }
                else
                {
                    lblFooterStatus.Text = msg;
                }
            };

            _alarmService.OnAlarmCleared += () =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        pnlVideoContainer.BackColor = _isDarkTheme ? Color.FromArgb(15, 23, 42) : Color.FromArgb(241, 245, 249);
                        lblFooterStatus.Text = "Hazır | Canlı Yüz Tanıma Aktif";
                    }));
                }
                else
                {
                    pnlVideoContainer.BackColor = _isDarkTheme ? Color.FromArgb(15, 23, 42) : Color.FromArgb(241, 245, 249);
                    lblFooterStatus.Text = "Hazır | Canlı Yüz Tanıma Aktif";
                }
            };

            // Kamera Başlat / Durdur
            btnStartCamera.Click += (s, e) =>
            {
                int selectedIndex = 0;
                if (cmbCameraDevice.SelectedItem is CameraDeviceItem item)
                {
                    selectedIndex = item.Index;
                }

                btnStartCamera.Enabled = false;
                _cameraService.Start(selectedIndex);
            };

            btnStopCamera.Click += (s, e) =>
            {
                _cameraService.Stop();
                _alarmService.Reset();
            };

            // Resim Seçme
            btnSelectImage.Click += BtnSelectImage_Click;

            // Kareyi Eğit
            btnCaptureSnapshot.Click += (s, e) =>
            {
                if (_lastRawFrame == null || _lastRawFrame.Empty())
                {
                    MessageBox.Show("Henüz kameradan veya dosyadan görüntü alınmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var faces = _detectionService.DetectFaces(_lastRawFrame);
                Mat? faceMat = null;
                if (faces.Length > 0)
                {
                    faceMat = _detectionService.ExtractNormalizedFace(_lastRawFrame, faces[0]);
                }

                using var trainDlg = new TrainFaceDialog(_detectionService, _recognitionService, faceMat);
                if (trainDlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshProfilesList();
                }
            };

            // Yeni Kişi Eğit Butonu
            btnOpenTrainDialog.Click += (s, e) =>
            {
                using var trainDlg = new TrainFaceDialog(_detectionService, _recognitionService, null);
                if (trainDlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshProfilesList();
                }
            };

            // Tema Değiştirme
            btnThemeToggle.Click += (s, e) =>
            {
                _isDarkTheme = !_isDarkTheme;
                ApplyTheme(_isDarkTheme);
            };

            // Özellik Tetikleyicileri
            chkVoiceAnnounce.CheckedChanged += (s, e) => _voiceService.IsEnabled = chkVoiceAnnounce.Checked;
            chkSecurityAlarm.CheckedChanged += (s, e) => _alarmService.IsEnabled = chkSecurityAlarm.Checked;

            // Hassasiyet Kaydırıcıları
            tbScaleFactor.Scroll += (s, e) => UpdateSensitivityLabels();
            tbNeighbors.Scroll += (s, e) => UpdateSensitivityLabels();

            // Rapor Kaydet
            btnExportLog.Click += (s, e) =>
            {
                if (_detectionHistory.Count == 0)
                {
                    MessageBox.Show("Kaydedilecek yüz algılama geçmişi bulunmuyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Metin Dosyası (*.txt)|*.txt",
                    FileName = $"FaceRecognition_Rapor_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                    Title = "Tanıma Geçmişi Raporunu Kaydet"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool ok = _storageService.ExportDetectionHistoryReport(sfd.FileName, _detectionHistory);
                    if (ok)
                        MessageBox.Show($"Rapor başarıyla kaydedildi:\n{sfd.FileName}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Dosya kaydedilirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Tabloyu Temizle
            btnClearDetections.Click += (s, e) =>
            {
                _detectionHistory.Clear();
                dgvDetections.Rows.Clear();
                grpDetections.Text = "🎯 Canlı Algılama ve Tanıma Geçmişi (0)";
            };

            // Varsayılan Profilleri Sıfırla
            btnResetProfiles.Click += (s, e) =>
            {
                if (MessageBox.Show("Varsayılan gönüllü profillerini (İsmail Özdemir, Ahmet Yılmaz, Ayşe Demir) yeniden yüklemek istiyor musunuz?", "Profilleri Sıfırla", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _recognitionService.ResetToDefaults();
                    RefreshProfilesList();
                }
            };
        }

        private void BtnSelectImage_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar (*.*)|*.*",
                Title = "Yüz Tespiti ve Tanıma İçin Resim Seçin"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _cameraService.Stop();

                    using var img = Cv2.ImRead(ofd.FileName);
                    if (img.Empty())
                    {
                        MessageBox.Show("Fotoğraf açılamadı veya dosya bozuk.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var clone = img.Clone();
                    ProcessSingleFrame(clone, 0.0);
                    clone.Dispose();

                    UpdateCameraStatusUI(CameraStatus.Stopped, "Fotoğraf Yüklendi");
                    lblFooterStatus.Text = $"Seçilen dosya işlendi: {Path.GetFileName(ofd.FileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Resim işlenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region --- GÖRÜNTÜ VE YÜZ İŞLEME DÖNGÜSÜ ---

        private void Camera_OnFrameCaptured(Mat frame, double fps)
        {
            if (IsDisposed)
            {
                frame.Dispose();
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        using (frame)
                        {
                            ProcessSingleFrame(frame, fps);
                        }
                    }));
                }
                catch
                {
                    frame.Dispose();
                }
                return;
            }

            using (frame)
            {
                ProcessSingleFrame(frame, fps);
            }
        }

        private void ProcessSingleFrame(Mat frame, double fps)
        {
            if (frame == null || frame.Empty() || IsDisposed) return;

            _lastRawFrame?.Dispose();
            _lastRawFrame = frame.Clone();

            if (fps > 0)
            {
                lblFps.Text = $"{fps:F1} FPS";
            }
            else
            {
                lblFps.Text = "Statik";
            }

            // 1. Çoklu Yüz Algılama (Haar Cascade)
            Rect[] faceBoxes = _detectionService.DetectFaces(frame);
            var currentFaces = new List<DetectedFace>();
            bool hasUnknownFace = false;

            foreach (var box in faceBoxes)
            {
                _detectionCounter++;
                var detected = new DetectedFace
                {
                    Id = _detectionCounter,
                    BoundingBox = box,
                    Timestamp = DateTime.Now
                };

                // 2. Normalize yüz çıkarımı ve Tanıma tahmini
                using var normalized = _detectionService.ExtractNormalizedFace(frame, box);
                if (normalized != null)
                {
                    detected.Recognition = _recognitionService.Predict(normalized);
                    detected.CropBitmap = BitmapConverter.ToBitmap(normalized);

                    // Canlılık (Göz) Tespiti
                    if (chkLiveness.Checked)
                    {
                        var eyes = _detectionService.DetectEyes(normalized);
                        detected.EyesCount = eyes.Length;
                        detected.IsLive = eyes.Length >= 1 || _cameraService.IsVirtualCamera;
                    }

                    // Gülümseme (Mimik) Tespiti
                    if (chkSmileDetect.Checked)
                    {
                        detected.IsSmiling = _detectionService.DetectSmile(normalized);
                    }

                    // Sesli Karşılama (TTS)
                    if (chkVoiceAnnounce.Checked)
                    {
                        if (detected.Recognition.IsRecognized)
                        {
                            _voiceService.AnnounceRecognized(detected.Recognition.Label);
                        }
                        else
                        {
                            _voiceService.AnnounceUnknown();
                        }
                    }

                    if (!detected.Recognition.IsRecognized)
                    {
                        hasUnknownFace = true;
                    }

                    // Otomatik Yüz Kaydı
                    if (chkAutoSaveFaces.Checked && (DateTime.Now - _lastAutoSaveTime).TotalSeconds >= 1.5)
                    {
                        _storageService.SaveCroppedFace(detected.CropBitmap, detected.Recognition.Label, detected.Recognition.Confidence);
                        _lastAutoSaveTime = DateTime.Now;
                    }
                }

                currentFaces.Add(detected);
                _detectionHistory.Insert(0, detected);

                // Tabloya ekle (Maksimum son 100 kayıt)
                dgvDetections.Rows.Insert(0, 
                    detected.Id, 
                    detected.Timestamp.ToString("HH:mm:ss"), 
                    detected.Recognition.Label, 
                    detected.Recognition.ConfidencePercentage,
                    detected.IsLive ? "✅" : "⏳",
                    detected.IsSmiling ? "😊 Tebessüm" : "Normal"
                );

                if (dgvDetections.Rows.Count > 100)
                {
                    dgvDetections.Rows.RemoveAt(dgvDetections.Rows.Count - 1);
                }
            }

            // Güvenlik Alarmı Değerlendirmesi
            if (chkSecurityAlarm.Checked)
            {
                using var currentBmp = BitmapConverter.ToBitmap(frame);
                _alarmService.EvaluateFrame(hasUnknownFace, currentBmp);

                if (_alarmService.IsAlarmActive)
                {
                    pnlVideoContainer.BackColor = Color.FromArgb(220, 38, 38); // Kırmızı alarm çerçevesi
                }
                else
                {
                    pnlVideoContainer.BackColor = _isDarkTheme ? Color.FromArgb(15, 23, 42) : Color.FromArgb(241, 245, 249);
                }
            }

            grpDetections.Text = $"🎯 Canlı Algılama ve Tanıma Geçmişi ({_detectionHistory.Count})";
            lblFaceCount.Text = $"👥 ({faceBoxes.Length})";

            // 3. Görsel çerçeveleri, canlılık ve mimik rozetlerini kareye çiz
            _detectionService.DrawFaceOverlays(frame, currentFaces, chkLiveness.Checked, chkSmileDetect.Checked);

            // 4. PictureBox üzerinde göster
            var oldBmp = pbVideoFeed.Image;
            pbVideoFeed.Image = BitmapConverter.ToBitmap(frame);
            oldBmp?.Dispose();
        }

        private void Camera_OnStatusChanged(CameraStatus status, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Camera_OnStatusChanged(status, message)));
                return;
            }

            UpdateCameraStatusUI(status, message);
        }

        private void UpdateCameraStatusUI(CameraStatus status, string message)
        {
            switch (status)
            {
                case CameraStatus.Running:
                    lblCameraStatus.Text = "● Canlı Yayında";
                    lblCameraStatus.BackColor = Color.FromArgb(22, 163, 74);
                    btnStartCamera.Enabled = false;
                    btnStopCamera.Enabled = true;
                    lblFooterStatus.Text = _cameraService.IsVirtualCamera 
                        ? "Sanal Test Kamerası devrede. Canlı yüz simülasyonu çalışıyor..."
                        : "Fiziksel kamera aktif. Canlı yüz algılama, canlılık ve tanıma çalışıyor...";
                    break;

                case CameraStatus.Stopped:
                    lblCameraStatus.Text = "■ Kamera Kapalı";
                    lblCameraStatus.BackColor = Color.FromArgb(100, 116, 139);
                    btnStartCamera.Enabled = true;
                    btnStopCamera.Enabled = false;
                    lblFps.Text = "0.0 FPS";
                    lblFooterStatus.Text = "Kamera kapalı. Başlatmak için 'Başlat' butonuna basınız.";
                    break;

                case CameraStatus.Error:
                    lblCameraStatus.Text = "⚠ Kamera Hatası";
                    lblCameraStatus.BackColor = Color.FromArgb(220, 38, 38);
                    btnStartCamera.Enabled = true;
                    btnStopCamera.Enabled = false;
                    lblFooterStatus.Text = "Kamera açılamadı. Dropdown'dan 'Sanal Kamera' seçerek testi hemen sürdürebilirsiniz.";
                    break;
            }
        }

        private void Camera_OnError(string error)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Camera_OnError(error)));
                return;
            }

            MessageBox.Show(error, "Kamera Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void Detection_OnError(string error)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Detection_OnError(error)));
                return;
            }

            MessageBox.Show(error, "Yüz Algılama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region --- TEMA YÖNETİMİ (DARK / LIGHT THEME) ---

        private void ApplyTheme(bool isDark)
        {
            btnThemeToggle.Text = isDark ? "☀️ Açık Mod" : "🌙 Koyu Mod";

            Color bgColor = isDark ? Color.FromArgb(15, 23, 42) : Color.FromArgb(248, 250, 252);
            Color panelColor = isDark ? Color.FromArgb(30, 41, 59) : Color.FromArgb(241, 245, 249);
            Color textColor = isDark ? Color.White : Color.FromArgb(15, 23, 42);
            Color subTextColor = isDark ? Color.FromArgb(203, 213, 225) : Color.FromArgb(51, 65, 85);
            Color groupBorderColor = isDark ? Color.FromArgb(51, 65, 85) : Color.FromArgb(203, 213, 225);

            this.BackColor = bgColor;
            pnlSideDashboard.BackColor = bgColor;
            pnlVideoContainer.BackColor = bgColor;

            // Grup kutuları
            GroupBox[] groups = { grpProfiles, grpFeatures, grpSettings, grpDetections };
            foreach (var grp in groups)
            {
                grp.ForeColor = textColor;
                grp.BackColor = bgColor;
            }

            // Checkbox'lar
            CheckBox[] checks = { chkVoiceAnnounce, chkLiveness, chkSmileDetect, chkSecurityAlarm, chkAutoSaveFaces };
            foreach (var chk in checks)
            {
                if (chk != chkSecurityAlarm) chk.ForeColor = subTextColor;
            }

            // Listeler & Tablolar
            lstProfiles.BackColor = panelColor;
            lstProfiles.ForeColor = textColor;

            dgvDetections.BackgroundColor = panelColor;
            dgvDetections.DefaultCellStyle.BackColor = panelColor;
            dgvDetections.DefaultCellStyle.ForeColor = textColor;
            dgvDetections.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(51, 65, 85) : Color.FromArgb(226, 232, 240);
            dgvDetections.ColumnHeadersDefaultCellStyle.ForeColor = textColor;

            // Etiketler
            lblScaleFactor.ForeColor = subTextColor;
            lblScaleFactorVal.ForeColor = textColor;
            lblNeighbors.ForeColor = subTextColor;
            lblNeighborsVal.ForeColor = textColor;

            statusStrip.BackColor = isDark ? Color.FromArgb(30, 41, 59) : Color.FromArgb(226, 232, 240);
            lblFooterStatus.ForeColor = subTextColor;
            lblFooterStudent.ForeColor = textColor;
        }

        #endregion

        private class CameraDeviceItem
        {
            public int Index { get; }
            public string Name { get; }

            public CameraDeviceItem(int index, string name)
            {
                Index = index;
                Name = name;
            }

            public override string ToString() => Name;
        }
    }
}
