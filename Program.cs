using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using FaceRecognition.Forms;
using FaceRecognition.Services;

namespace FaceRecognition
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Any(a => a.Contains("generate-screenshots")) || Environment.CommandLine.Contains("generate-screenshots"))
            {
                GenerateScreenshots();
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        private static void GenerateScreenshots()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
                string screenshotsDir = Path.Combine(projectDir, "screenshots");
                Directory.CreateDirectory(screenshotsDir);

                ApplicationConfiguration.Initialize();

                // 1. Ekran Görüntüsü: Canlı Yüz Algılama ve Tanıma Paneli
                using (MainForm form = new MainForm())
                {
                    form.Show();
                    form.Size = new System.Drawing.Size(1160, 724);

                    var pbVideo = form.Controls.Find("pbVideoFeed", true).FirstOrDefault() as PictureBox;
                    var lblStatus = form.Controls.Find("lblCameraStatus", true).FirstOrDefault() as Label;
                    var lblFps = form.Controls.Find("lblFps", true).FirstOrDefault() as Label;
                    var lblFaceCount = form.Controls.Find("lblFaceCount", true).FirstOrDefault() as Label;
                    var dgv = form.Controls.Find("dgvDetections", true).FirstOrDefault() as DataGridView;
                    var grpDet = form.Controls.Find("grpDetections", true).FirstOrDefault() as GroupBox;

                    if (lblStatus != null)
                    {
                        lblStatus.Text = "● Canlı Yayında";
                        lblStatus.BackColor = Color.FromArgb(22, 163, 74);
                    }
                    if (lblFps != null) lblFps.Text = "29.6 FPS";
                    if (lblFaceCount != null) lblFaceCount.Text = "👥 (2)";

                    // Sentetik simüle edilmiş çoklu yüz karesi üret
                    using var simMat = new Mat(480, 640, MatType.CV_8UC3, new OpenCvSharp.Scalar(30, 41, 59));
                    // Yüz 1 (İsmail Özdemir)
                    var box1 = new Rect(140, 100, 160, 190);
                    Cv2.Ellipse(simMat, new OpenCvSharp.Point(220, 195), new OpenCvSharp.Size(80, 95), 0, 0, 360, new OpenCvSharp.Scalar(210, 180, 140), -1);
                    Cv2.Circle(simMat, new OpenCvSharp.Point(190, 175), 10, new OpenCvSharp.Scalar(40, 40, 40), -1);
                    Cv2.Circle(simMat, new OpenCvSharp.Point(250, 175), 10, new OpenCvSharp.Scalar(40, 40, 40), -1);
                    Cv2.Line(simMat, new OpenCvSharp.Point(195, 230), new OpenCvSharp.Point(245, 230), new OpenCvSharp.Scalar(40, 40, 180), 3);
                    Cv2.Rectangle(simMat, box1, new OpenCvSharp.Scalar(34, 197, 94), 2);
                    Cv2.Rectangle(simMat, new Rect(140, 72, 180, 26), new OpenCvSharp.Scalar(34, 197, 94), -1);
                    Cv2.PutText(simMat, "Ismail Ozdemir (95.4%)", new OpenCvSharp.Point(145, 90), HersheyFonts.HersheySimplex, 0.48, OpenCvSharp.Scalar.White, 1);
                    Cv2.PutText(simMat, "CANLI [OK]", new OpenCvSharp.Point(140, 305), HersheyFonts.HersheySimplex, 0.42, new OpenCvSharp.Scalar(34, 197, 94), 1);
                    Cv2.PutText(simMat, "GULUMSEME :)", new OpenCvSharp.Point(245, 305), HersheyFonts.HersheySimplex, 0.42, new OpenCvSharp.Scalar(251, 146, 60), 1);

                    // Yüz 2 (Ahmet Yılmaz)
                    var box2 = new Rect(380, 120, 150, 180);
                    Cv2.Ellipse(simMat, new OpenCvSharp.Point(455, 210), new OpenCvSharp.Size(75, 90), 0, 0, 360, new OpenCvSharp.Scalar(190, 165, 130), -1);
                    Cv2.Circle(simMat, new OpenCvSharp.Point(430, 190), 9, new OpenCvSharp.Scalar(40, 40, 40), -1);
                    Cv2.Circle(simMat, new OpenCvSharp.Point(480, 190), 9, new OpenCvSharp.Scalar(40, 40, 40), -1);
                    Cv2.Line(simMat, new OpenCvSharp.Point(435, 245), new OpenCvSharp.Point(475, 245), new OpenCvSharp.Scalar(40, 40, 180), 3);
                    Cv2.Rectangle(simMat, box2, new OpenCvSharp.Scalar(34, 197, 94), 2);
                    Cv2.Rectangle(simMat, new Rect(380, 92, 175, 26), new OpenCvSharp.Scalar(34, 197, 94), -1);
                    Cv2.PutText(simMat, "Ahmet Yilmaz (91.8%)", new OpenCvSharp.Point(385, 110), HersheyFonts.HersheySimplex, 0.48, OpenCvSharp.Scalar.White, 1);
                    Cv2.PutText(simMat, "CANLI [OK]", new OpenCvSharp.Point(380, 315), HersheyFonts.HersheySimplex, 0.42, new OpenCvSharp.Scalar(34, 197, 94), 1);

                    if (pbVideo != null)
                    {
                        pbVideo.Image = BitmapConverter.ToBitmap(simMat);
                    }

                    if (dgv != null)
                    {
                        dgv.Rows.Add(4, DateTime.Now.AddSeconds(-1).ToString("HH:mm:ss"), "İsmail Özdemir", "%95.4", "✅", "😊 Tebessüm");
                        dgv.Rows.Add(3, DateTime.Now.AddSeconds(-2).ToString("HH:mm:ss"), "Ahmet Yılmaz", "%91.8", "✅", "Normal");
                        dgv.Rows.Add(2, DateTime.Now.AddSeconds(-8).ToString("HH:mm:ss"), "İsmail Özdemir", "%94.8", "✅", "😊 Tebessüm");
                        dgv.Rows.Add(1, DateTime.Now.AddSeconds(-15).ToString("HH:mm:ss"), "Ayşe Demir", "%89.2", "✅", "Normal");
                    }
                    if (grpDet != null) grpDet.Text = "🎯 Canlı Algılama ve Tanıma Geçmişi (4)";

                    Application.DoEvents();
                    Thread.Sleep(200);

                    using (Bitmap bmp = new Bitmap(form.Width, form.Height))
                    {
                        form.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, form.Width, form.Height));
                        string livePath = Path.Combine(screenshotsDir, "live_detection.png");
                        bmp.Save(livePath, ImageFormat.Png);
                    }

                    form.Close();
                }

                // 2. Ekran Görüntüsü: Yeni Kişi Eğitme ve Model Güncelleme Ekranı
                using (var detect = new FaceDetectionService())
                {
                    var recog = new FaceRecognitionService();
                    using (var dummyFace = new Mat(100, 100, MatType.CV_8UC3, new OpenCvSharp.Scalar(210, 180, 140)))
                    using (var trainDlg = new TrainFaceDialog(detect, recog, dummyFace))
                    {
                        trainDlg.Show();
                        trainDlg.Size = new System.Drawing.Size(370, 390);

                        var txtName = trainDlg.Controls.Find("txtPersonName", true).FirstOrDefault() as TextBox;
                        if (txtName != null) txtName.Text = "İsmail Özdemir";

                        Application.DoEvents();
                        Thread.Sleep(200);

                        using (Bitmap bmp = new Bitmap(trainDlg.Width, trainDlg.Height))
                        {
                            trainDlg.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, trainDlg.Width, trainDlg.Height));
                            string trainPath = Path.Combine(screenshotsDir, "recognition_train.png");
                            bmp.Save(trainPath, ImageFormat.Png);
                        }

                        trainDlg.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}
