using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using FaceRecognition.Services;

namespace FaceRecognition.Forms
{
    public partial class TrainFaceDialog : Form
    {
        private readonly FaceDetectionService _detectionService;
        private readonly FaceRecognitionService _recognitionService;
        private Mat? _normalizedFaceMat;

        public string PersonName => txtPersonName.Text.Trim();

        public TrainFaceDialog(FaceDetectionService detectionService, FaceRecognitionService recognitionService, Mat? initialFace = null)
        {
            InitializeComponent();

            _detectionService = detectionService;
            _recognitionService = recognitionService;

            if (initialFace != null && !initialFace.Empty())
            {
                SetDetectedFace(initialFace);
            }

            btnSelectImage.Click += BtnSelectImage_Click;
            btnSaveProfile.Click += BtnSaveProfile_Click;
            txtPersonName.TextChanged += (s, e) => ValidateInputs();
        }

        private void SetDetectedFace(Mat faceMat)
        {
            _normalizedFaceMat?.Dispose();
            _normalizedFaceMat = faceMat.Clone();

            pbFacePreview.Image?.Dispose();
            pbFacePreview.Image = BitmapConverter.ToBitmap(_normalizedFaceMat);
            lblPreviewInfo.Text = "✅ Yüz başarıyla kırpıldı ve normalize edildi.";
            lblPreviewInfo.ForeColor = Color.FromArgb(22, 163, 74);
            ValidateInputs();
        }

        private void BtnSelectImage_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar (*.*)|*.*",
                Title = "Eğitilecek Yüz Fotoğrafını Seçin"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var img = Cv2.ImRead(ofd.FileName);
                    if (img.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi veya bozuk dosya.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var faces = _detectionService.DetectFaces(img);
                    if (faces.Length == 0)
                    {
                        MessageBox.Show("Seçilen fotoğrafta herhangi bir insan yüzü algılanamadı.\nLütfen yüzün net göründüğü bir fotoğraf seçiniz.", "Yüz Algılanamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // En büyük yüzü al
                    Rect bestFace = faces[0];
                    for (int i = 1; i < faces.Length; i++)
                    {
                        if (faces[i].Width * faces[i].Height > bestFace.Width * bestFace.Height)
                            bestFace = faces[i];
                    }

                    var normalized = _detectionService.ExtractNormalizedFace(img, bestFace);
                    if (normalized != null)
                    {
                        SetDetectedFace(normalized);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fotoğraf işlenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ValidateInputs()
        {
            btnSaveProfile.Enabled = !string.IsNullOrWhiteSpace(txtPersonName.Text) && _normalizedFaceMat != null && !_normalizedFaceMat.Empty();
        }

        private void BtnSaveProfile_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PersonName) || _normalizedFaceMat == null) return;

            bool ok = _recognitionService.TrainPerson(PersonName, _normalizedFaceMat);
            if (ok)
            {
                MessageBox.Show($"'{PersonName}' profili başarıyla sisteme eğitildi ve veri setine eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Eğitim sırasında bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _normalizedFaceMat?.Dispose();
            pbFacePreview.Image?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
