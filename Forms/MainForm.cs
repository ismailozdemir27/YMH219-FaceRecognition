using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        private Mat? _lastRawFrame;
        private readonly List<DetectedFace> _detectionHistory = new List<DetectedFace>();
        private int _detectionCounter = 0;
        private DateTime _lastAutoSaveTime = DateTime.MinValue;

        public MainForm()
        {
            InitializeComponent();

            _detectionService = new FaceDetectionService();
            _recognitionService = new FaceRecognitionService();
            _cameraService = new CameraService();
            _storageService = new FileStorageService();

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

            if (!_detectionService.IsLoaded)
            {
                lblFooterStatus.Text = "⚠ Haar Cascade model dosyası yüklenemedi!";
                lblFooterStatus.ForeColor = Color.Red;
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _cameraService.Stop();
            _cameraService.Dispose();
            _detectionService.Dispose();
            _lastRawFrame?.Dispose();
            pbVideoFeed.Image?.Dispose();
        }

        #region --- GRİD VE LİSTE BAĞLANTILARI ---

        private void InitializeDetectionsGrid()
        {
            dgvDetections.Columns.Clear();

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "#",
                Width = 40
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTime",
                HeaderText = "Zaman",
                Width = 90
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Kişi / Etiket",
                Width = 140
            });

            dgvDetections.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colConfidence",
                HeaderText = "Güven",
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

            // Kamera Başlat / Durdur
            btnStartCamera.Click += (s, e) =>
            {
                btnStartCamera.Enabled = false;
                _cameraService.Start(0);
            };

            btnStopCamera.Click += (s, e) =>
            {
                _cameraService.Stop();
            };

            // Görüntü Dosyası Seçme
            btnSelectImage.Click += BtnSelectImage_Click;

            // Kare Yakala & Eğit
            btnCaptureSnapshot.Click += (s, e) =>
            {
                if (_lastRawFrame == null || _lastRawFrame.Empty())
                {
                    MessageBox.Show("Henüz kameradan veya dosyadan görüntü alınmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Yakalanan kareden yüz tespit et
                var faces = _detectionService.DetectFaces(_lastRawFrame);
                Mat? faceMat = null;
                if (faces.Length > 0)
                {
                    faceMat = _detectionService.ExtractNormalizedFace(_lastRawFrame, faces[0]);
                }

                using var dlg = new TrainFaceDialog(_detectionService, _recognitionService, faceMat);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshProfilesList();
                }
            };

            // Yeni Kişi Eğit Butonu
            btnOpenTrainDialog.Click += (s, e) =>
            {
                using var dlg = new TrainFaceDialog(_detectionService, _recognitionService);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshProfilesList();
                }
            };

            // Hassasiyet kaydırıcıları
            tbScaleFactor.ValueChanged += (s, e) => UpdateSensitivityLabels();
            tbNeighbors.ValueChanged += (s, e) => UpdateSensitivityLabels();

            // Profilleri sıfırlama
            btnResetProfiles.Click += (s, e) =>
            {
                if (MessageBox.Show("Varsayılan gönüllü profillerini (İsmail Özdemir, Ahmet Yılmaz, Ayşe Demir) yeniden yüklemek istiyor musunuz?", "Profilleri Sıfırla", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _recognitionService.ClearAllProfiles();
                    _recognitionService.SeedDefaultVolunteerProfiles();
                    RefreshProfilesList();
                }
            };

            // Rapor dışa aktar
            btnExportLog.Click += (s, e) =>
            {
                if (_detectionHistory.Count == 0)
                {
                    MessageBox.Show("Kaydedilecek yüz algılama geçmişi bulunmuyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Metin Belgesi (*.txt)|*.txt",
                    FileName = $"YuzTanima_Raporu_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                    Title = "Yüz Tanıma Raporunu Kaydet"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (_storageService.SaveRecognitionLogToFile(sfd.FileName, _detectionHistory))
                    {
                        MessageBox.Show($"Rapor başarıyla kaydedildi:\n{sfd.FileName}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Dosya kaydedilirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            // Tabloyu temizle
            btnClearDetections.Click += (s, e) =>
            {
                _detectionHistory.Clear();
                dgvDetections.Rows.Clear();
                _detectionCounter = 0;
                grpDetections.Text = "🎯 Canlı Algılama ve Tanıma Geçmişi (0)";
            };
        }

        private void BtnSelectImage_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar (*.*)|*.*",
                Title = "İncelenecek Fotoğrafı Seçin"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Kamera açıksa durdur
                if (_cameraService.IsRunning)
                {
                    _cameraService.Stop();
                }

                try
                {
                    using var img = Cv2.ImRead(ofd.FileName);
                    if (img.Empty())
                    {
                        MessageBox.Show("Fotoğraf açılamadı veya dosya bozuk.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Statik resmi işle
                    ProcessSingleFrame(img, 0.0);
                    lblFooterStatus.Text = $"Dosya incelendi: {Path.GetFileName(ofd.FileName)}";
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
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(() => ProcessSingleFrame(frame, fps)));
                }
                catch { }
                return;
            }

            ProcessSingleFrame(frame, fps);
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

                    // İsteğe bağlı ek özellik: Algılanan yüzü klasöre kaydetme
                    if (chkAutoSaveFaces.Checked && (DateTime.Now - _lastAutoSaveTime).TotalSeconds >= 1.5)
                    {
                        _storageService.SaveCroppedFace(detected.CropBitmap, detected.Recognition.Label, detected.Recognition.Confidence);
                        _lastAutoSaveTime = DateTime.Now;
                    }
                }

                currentFaces.Add(detected);
                _detectionHistory.Insert(0, detected);

                // Tabloya ekle (Maksimum son 100 kayıt)
                dgvDetections.Rows.Insert(0, detected.Id, detected.Timestamp.ToString("HH:mm:ss"), detected.Recognition.Label, detected.Recognition.ConfidencePercentage);
                if (dgvDetections.Rows.Count > 100)
                {
                    dgvDetections.Rows.RemoveAt(dgvDetections.Rows.Count - 1);
                }
            }

            grpDetections.Text = $"🎯 Canlı Algılama ve Tanıma Geçmişi ({_detectionHistory.Count})";
            lblFaceCount.Text = $"👥 ({faceBoxes.Length})";

            // 3. Görsel çerçeveleri ve etiketleri kareye çiz
            _detectionService.DrawFaceOverlays(frame, currentFaces);

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
                    lblFooterStatus.Text = "Kamera aktif. Canlı yüz algılama ve tanıma çalışıyor...";
                    break;

                case CameraStatus.Stopped:
                    lblCameraStatus.Text = "■ Kamera Kapalı";
                    lblCameraStatus.BackColor = Color.FromArgb(100, 116, 139);
                    btnStartCamera.Enabled = true;
                    btnStopCamera.Enabled = false;
                    lblFps.Text = "0.0 FPS";
                    lblFooterStatus.Text = "Kamera kapalı. Başlatmak için 'Kamera Başlat' butonuna basınız.";
                    break;

                case CameraStatus.Error:
                    lblCameraStatus.Text = "⚠ Kamera Hatası";
                    lblCameraStatus.BackColor = Color.FromArgb(220, 38, 38);
                    btnStartCamera.Enabled = true;
                    btnStopCamera.Enabled = false;
                    lblFooterStatus.Text = "Kamera açılamadı. 'Görüntü Seç' butonu ile fotoğraf testi yapabilirsiniz.";
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
    }
}
