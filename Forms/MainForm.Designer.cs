namespace FaceRecognition.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.ComboBox cmbCameraDevice;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.Button btnStopCamera;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnCaptureSnapshot;
        private System.Windows.Forms.Button btnOpenTrainDialog;
        private System.Windows.Forms.Button btnThemeToggle;
        private System.Windows.Forms.Label lblCameraStatus;
        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.Label lblFaceCount;

        private System.Windows.Forms.Panel pnlVideoContainer;
        private System.Windows.Forms.PictureBox pbVideoFeed;

        private System.Windows.Forms.Panel pnlSideDashboard;
        private System.Windows.Forms.GroupBox grpProfiles;
        private System.Windows.Forms.ListBox lstProfiles;
        private System.Windows.Forms.Button btnResetProfiles;

        private System.Windows.Forms.GroupBox grpFeatures;
        private System.Windows.Forms.CheckBox chkVoiceAnnounce;
        private System.Windows.Forms.CheckBox chkLiveness;
        private System.Windows.Forms.CheckBox chkSmileDetect;
        private System.Windows.Forms.CheckBox chkSecurityAlarm;
        private System.Windows.Forms.CheckBox chkAutoSaveFaces;

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Label lblScaleFactor;
        private System.Windows.Forms.TrackBar tbScaleFactor;
        private System.Windows.Forms.Label lblScaleFactorVal;
        private System.Windows.Forms.Label lblNeighbors;
        private System.Windows.Forms.TrackBar tbNeighbors;
        private System.Windows.Forms.Label lblNeighborsVal;

        private System.Windows.Forms.GroupBox grpDetections;
        private System.Windows.Forms.DataGridView dgvDetections;

        private System.Windows.Forms.Panel pnlBottomActions;
        private System.Windows.Forms.Button btnExportLog;
        private System.Windows.Forms.Button btnClearDetections;

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblFooterStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblFooterStudent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.cmbCameraDevice = new System.Windows.Forms.ComboBox();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.btnStopCamera = new System.Windows.Forms.Button();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.btnCaptureSnapshot = new System.Windows.Forms.Button();
            this.btnOpenTrainDialog = new System.Windows.Forms.Button();
            this.btnThemeToggle = new System.Windows.Forms.Button();
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.lblFps = new System.Windows.Forms.Label();
            this.lblFaceCount = new System.Windows.Forms.Label();

            this.pnlVideoContainer = new System.Windows.Forms.Panel();
            this.pbVideoFeed = new System.Windows.Forms.PictureBox();

            this.pnlSideDashboard = new System.Windows.Forms.Panel();
            this.grpProfiles = new System.Windows.Forms.GroupBox();
            this.lstProfiles = new System.Windows.Forms.ListBox();
            this.btnResetProfiles = new System.Windows.Forms.Button();

            this.grpFeatures = new System.Windows.Forms.GroupBox();
            this.chkVoiceAnnounce = new System.Windows.Forms.CheckBox();
            this.chkLiveness = new System.Windows.Forms.CheckBox();
            this.chkSmileDetect = new System.Windows.Forms.CheckBox();
            this.chkSecurityAlarm = new System.Windows.Forms.CheckBox();
            this.chkAutoSaveFaces = new System.Windows.Forms.CheckBox();

            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.lblScaleFactor = new System.Windows.Forms.Label();
            this.tbScaleFactor = new System.Windows.Forms.TrackBar();
            this.lblScaleFactorVal = new System.Windows.Forms.Label();
            this.lblNeighbors = new System.Windows.Forms.Label();
            this.tbNeighbors = new System.Windows.Forms.TrackBar();
            this.lblNeighborsVal = new System.Windows.Forms.Label();

            this.grpDetections = new System.Windows.Forms.GroupBox();
            this.dgvDetections = new System.Windows.Forms.DataGridView();

            this.pnlBottomActions = new System.Windows.Forms.Panel();
            this.btnExportLog = new System.Windows.Forms.Button();
            this.btnClearDetections = new System.Windows.Forms.Button();

            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblFooterStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFooterStudent = new System.Windows.Forms.ToolStripStatusLabel();

            this.pnlTopBar.SuspendLayout();
            this.pnlVideoContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoFeed)).BeginInit();
            this.pnlSideDashboard.SuspendLayout();
            this.grpProfiles.SuspendLayout();
            this.grpFeatures.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbScaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNeighbors)).BeginInit();
            this.grpDetections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetections)).BeginInit();
            this.pnlBottomActions.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlTopBar.Controls.Add(this.cmbCameraDevice);
            this.pnlTopBar.Controls.Add(this.btnStartCamera);
            this.pnlTopBar.Controls.Add(this.btnStopCamera);
            this.pnlTopBar.Controls.Add(this.btnSelectImage);
            this.pnlTopBar.Controls.Add(this.btnCaptureSnapshot);
            this.pnlTopBar.Controls.Add(this.btnOpenTrainDialog);
            this.pnlTopBar.Controls.Add(this.btnThemeToggle);
            this.pnlTopBar.Controls.Add(this.lblCameraStatus);
            this.pnlTopBar.Controls.Add(this.lblFps);
            this.pnlTopBar.Controls.Add(this.lblFaceCount);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(1160, 60);
            this.pnlTopBar.TabIndex = 0;

            // 
            // cmbCameraDevice
            // 
            this.cmbCameraDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraDevice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.cmbCameraDevice.FormattingEnabled = true;
            this.cmbCameraDevice.Location = new System.Drawing.Point(12, 17);
            this.cmbCameraDevice.Name = "cmbCameraDevice";
            this.cmbCameraDevice.Size = new System.Drawing.Size(185, 23);
            this.cmbCameraDevice.TabIndex = 0;

            // 
            // btnStartCamera
            // 
            this.btnStartCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnStartCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartCamera.FlatAppearance.BorderSize = 0;
            this.btnStartCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartCamera.ForeColor = System.Drawing.Color.White;
            this.btnStartCamera.Location = new System.Drawing.Point(205, 13);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(95, 34);
            this.btnStartCamera.TabIndex = 1;
            this.btnStartCamera.Text = "▶ Başlat";
            this.btnStartCamera.UseVisualStyleBackColor = false;

            // 
            // btnStopCamera
            // 
            this.btnStopCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnStopCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopCamera.Enabled = false;
            this.btnStopCamera.FlatAppearance.BorderSize = 0;
            this.btnStopCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopCamera.ForeColor = System.Drawing.Color.White;
            this.btnStopCamera.Location = new System.Drawing.Point(306, 13);
            this.btnStopCamera.Name = "btnStopCamera";
            this.btnStopCamera.Size = new System.Drawing.Size(90, 34);
            this.btnStopCamera.TabIndex = 2;
            this.btnStopCamera.Text = "⏹ Durdur";
            this.btnStopCamera.UseVisualStyleBackColor = false;

            // 
            // btnSelectImage
            // 
            this.btnSelectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSelectImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectImage.FlatAppearance.BorderSize = 0;
            this.btnSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSelectImage.ForeColor = System.Drawing.Color.White;
            this.btnSelectImage.Location = new System.Drawing.Point(402, 13);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(105, 34);
            this.btnSelectImage.TabIndex = 3;
            this.btnSelectImage.Text = "📂 Resim Seç";
            this.btnSelectImage.UseVisualStyleBackColor = false;

            // 
            // btnCaptureSnapshot
            // 
            this.btnCaptureSnapshot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(148)))), ((int)(((byte)(136)))));
            this.btnCaptureSnapshot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureSnapshot.FlatAppearance.BorderSize = 0;
            this.btnCaptureSnapshot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureSnapshot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCaptureSnapshot.ForeColor = System.Drawing.Color.White;
            this.btnCaptureSnapshot.Location = new System.Drawing.Point(513, 13);
            this.btnCaptureSnapshot.Name = "btnCaptureSnapshot";
            this.btnCaptureSnapshot.Size = new System.Drawing.Size(115, 34);
            this.btnCaptureSnapshot.TabIndex = 4;
            this.btnCaptureSnapshot.Text = "📸 Kareyi Eğit";
            this.btnCaptureSnapshot.UseVisualStyleBackColor = false;

            // 
            // btnOpenTrainDialog
            // 
            this.btnOpenTrainDialog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.btnOpenTrainDialog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenTrainDialog.FlatAppearance.BorderSize = 0;
            this.btnOpenTrainDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenTrainDialog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpenTrainDialog.ForeColor = System.Drawing.Color.White;
            this.btnOpenTrainDialog.Location = new System.Drawing.Point(634, 13);
            this.btnOpenTrainDialog.Name = "btnOpenTrainDialog";
            this.btnOpenTrainDialog.Size = new System.Drawing.Size(105, 34);
            this.btnOpenTrainDialog.TabIndex = 5;
            this.btnOpenTrainDialog.Text = "➕ Yeni Kişi";
            this.btnOpenTrainDialog.UseVisualStyleBackColor = false;

            // 
            // btnThemeToggle
            // 
            this.btnThemeToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnThemeToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemeToggle.FlatAppearance.BorderSize = 0;
            this.btnThemeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemeToggle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnThemeToggle.ForeColor = System.Drawing.Color.White;
            this.btnThemeToggle.Location = new System.Drawing.Point(745, 13);
            this.btnThemeToggle.Name = "btnThemeToggle";
            this.btnThemeToggle.Size = new System.Drawing.Size(105, 34);
            this.btnThemeToggle.TabIndex = 6;
            this.btnThemeToggle.Text = "🌙 Koyu Mod";
            this.btnThemeToggle.UseVisualStyleBackColor = false;

            // 
            // lblCameraStatus
            // 
            this.lblCameraStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCameraStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCameraStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCameraStatus.ForeColor = System.Drawing.Color.White;
            this.lblCameraStatus.Location = new System.Drawing.Point(865, 16);
            this.lblCameraStatus.Name = "lblCameraStatus";
            this.lblCameraStatus.Size = new System.Drawing.Size(125, 28);
            this.lblCameraStatus.TabIndex = 7;
            this.lblCameraStatus.Text = "■ Kamera Kapalı";
            this.lblCameraStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblFps
            // 
            this.lblFps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFps.AutoSize = true;
            this.lblFps.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblFps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblFps.Location = new System.Drawing.Point(998, 22);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(43, 15);
            this.lblFps.TabIndex = 8;
            this.lblFps.Text = "0.0 FPS";

            // 
            // lblFaceCount
            // 
            this.lblFaceCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFaceCount.AutoSize = true;
            this.lblFaceCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblFaceCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(191)))), ((int)(((byte)(36)))));
            this.lblFaceCount.Location = new System.Drawing.Point(1055, 22);
            this.lblFaceCount.Name = "lblFaceCount";
            this.lblFaceCount.Size = new System.Drawing.Size(40, 15);
            this.lblFaceCount.TabIndex = 9;
            this.lblFaceCount.Text = "👥 (0)";

            // 
            // pnlSideDashboard
            // 
            this.pnlSideDashboard.Controls.Add(this.grpDetections);
            this.pnlSideDashboard.Controls.Add(this.pnlBottomActions);
            this.pnlSideDashboard.Controls.Add(this.grpSettings);
            this.pnlSideDashboard.Controls.Add(this.grpFeatures);
            this.pnlSideDashboard.Controls.Add(this.grpProfiles);
            this.pnlSideDashboard.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSideDashboard.Location = new System.Drawing.Point(770, 60);
            this.pnlSideDashboard.Name = "pnlSideDashboard";
            this.pnlSideDashboard.Size = new System.Drawing.Size(390, 640);
            this.pnlSideDashboard.Padding = new System.Windows.Forms.Padding(0, 8, 8, 8);
            this.pnlSideDashboard.TabIndex = 2;

            // 
            // grpProfiles
            // 
            this.grpProfiles.Controls.Add(this.btnResetProfiles);
            this.grpProfiles.Controls.Add(this.lstProfiles);
            this.grpProfiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProfiles.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.grpProfiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpProfiles.Location = new System.Drawing.Point(0, 0);
            this.grpProfiles.Name = "grpProfiles";
            this.grpProfiles.Size = new System.Drawing.Size(382, 115);
            this.grpProfiles.TabIndex = 0;
            this.grpProfiles.TabStop = false;
            this.grpProfiles.Text = "👤 Eğitilmiş Gönüllü Yüz Profilleri (3)";

            // 
            // lstProfiles
            // 
            this.lstProfiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstProfiles.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lstProfiles.FormattingEnabled = true;
            this.lstProfiles.ItemHeight = 13;
            this.lstProfiles.Location = new System.Drawing.Point(3, 18);
            this.lstProfiles.Name = "lstProfiles";
            this.lstProfiles.Size = new System.Drawing.Size(376, 56);
            this.lstProfiles.TabIndex = 0;

            // 
            // btnResetProfiles
            // 
            this.btnResetProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnResetProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetProfiles.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnResetProfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetProfiles.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnResetProfiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnResetProfiles.Location = new System.Drawing.Point(6, 80);
            this.btnResetProfiles.Name = "btnResetProfiles";
            this.btnResetProfiles.Size = new System.Drawing.Size(370, 26);
            this.btnResetProfiles.TabIndex = 1;
            this.btnResetProfiles.Text = "🔄 Varsayılan Gönüllüleri Yeniden Yükle";
            this.btnResetProfiles.UseVisualStyleBackColor = false;

            // 
            // grpFeatures
            // 
            this.grpFeatures.Controls.Add(this.chkVoiceAnnounce);
            this.grpFeatures.Controls.Add(this.chkLiveness);
            this.grpFeatures.Controls.Add(this.chkSmileDetect);
            this.grpFeatures.Controls.Add(this.chkSecurityAlarm);
            this.grpFeatures.Controls.Add(this.chkAutoSaveFaces);
            this.grpFeatures.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFeatures.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.grpFeatures.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpFeatures.Location = new System.Drawing.Point(0, 115);
            this.grpFeatures.Name = "grpFeatures";
            this.grpFeatures.Size = new System.Drawing.Size(382, 110);
            this.grpFeatures.TabIndex = 1;
            this.grpFeatures.TabStop = false;
            this.grpFeatures.Text = "🚀 İleri Güvenlik & Yapay Zeka Özellikleri";

            // 
            // chkVoiceAnnounce
            // 
            this.chkVoiceAnnounce.AutoSize = true;
            this.chkVoiceAnnounce.Checked = true;
            this.chkVoiceAnnounce.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVoiceAnnounce.Font = new System.Drawing.Font("Segoe UI", 8.2F);
            this.chkVoiceAnnounce.Location = new System.Drawing.Point(10, 24);
            this.chkVoiceAnnounce.Name = "chkVoiceAnnounce";
            this.chkVoiceAnnounce.Size = new System.Drawing.Size(155, 17);
            this.chkVoiceAnnounce.TabIndex = 0;
            this.chkVoiceAnnounce.Text = "🔊 Sesli Karşılama (TTS)";
            this.chkVoiceAnnounce.UseVisualStyleBackColor = true;

            // 
            // chkLiveness
            // 
            this.chkLiveness.AutoSize = true;
            this.chkLiveness.Checked = true;
            this.chkLiveness.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLiveness.Font = new System.Drawing.Font("Segoe UI", 8.2F);
            this.chkLiveness.Location = new System.Drawing.Point(195, 24);
            this.chkLiveness.Name = "chkLiveness";
            this.chkLiveness.Size = new System.Drawing.Size(150, 17);
            this.chkLiveness.TabIndex = 1;
            this.chkLiveness.Text = "🛡️ Canlılık Tespiti (Anti-Spoof)";
            this.chkLiveness.UseVisualStyleBackColor = true;

            // 
            // chkSmileDetect
            // 
            this.chkSmileDetect.AutoSize = true;
            this.chkSmileDetect.Checked = true;
            this.chkSmileDetect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSmileDetect.Font = new System.Drawing.Font("Segoe UI", 8.2F);
            this.chkSmileDetect.Location = new System.Drawing.Point(10, 52);
            this.chkSmileDetect.Name = "chkSmileDetect";
            this.chkSmileDetect.Size = new System.Drawing.Size(145, 17);
            this.chkSmileDetect.TabIndex = 2;
            this.chkSmileDetect.Text = "😊 Gülümseme Tespiti";
            this.chkSmileDetect.UseVisualStyleBackColor = true;

            // 
            // chkSecurityAlarm
            // 
            this.chkSecurityAlarm.AutoSize = true;
            this.chkSecurityAlarm.Font = new System.Drawing.Font("Segoe UI", 8.2F, System.Drawing.FontStyle.Bold);
            this.chkSecurityAlarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.chkSecurityAlarm.Location = new System.Drawing.Point(195, 52);
            this.chkSecurityAlarm.Name = "chkSecurityAlarm";
            this.chkSecurityAlarm.Size = new System.Drawing.Size(152, 17);
            this.chkSecurityAlarm.TabIndex = 3;
            this.chkSecurityAlarm.Text = "🚨 Güvenlik Alarm Modu";
            this.chkSecurityAlarm.UseVisualStyleBackColor = true;

            // 
            // chkAutoSaveFaces
            // 
            this.chkAutoSaveFaces.AutoSize = true;
            this.chkAutoSaveFaces.Font = new System.Drawing.Font("Segoe UI", 8.2F);
            this.chkAutoSaveFaces.Location = new System.Drawing.Point(10, 80);
            this.chkAutoSaveFaces.Name = "chkAutoSaveFaces";
            this.chkAutoSaveFaces.Size = new System.Drawing.Size(190, 17);
            this.chkAutoSaveFaces.TabIndex = 4;
            this.chkAutoSaveFaces.Text = "💾 Yüzleri Klasöre Otomatik Kaydet";
            this.chkAutoSaveFaces.UseVisualStyleBackColor = true;

            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.lblNeighborsVal);
            this.grpSettings.Controls.Add(this.tbNeighbors);
            this.grpSettings.Controls.Add(this.lblNeighbors);
            this.grpSettings.Controls.Add(this.lblScaleFactorVal);
            this.grpSettings.Controls.Add(this.tbScaleFactor);
            this.grpSettings.Controls.Add(this.lblScaleFactor);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.grpSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpSettings.Location = new System.Drawing.Point(0, 225);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(382, 105);
            this.grpSettings.TabIndex = 2;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "⚙️ Algılama Hassasiyeti (Haar Parametreleri)";

            // 
            // lblScaleFactor
            // 
            this.lblScaleFactor.AutoSize = true;
            this.lblScaleFactor.Font = new System.Drawing.Font("Segoe UI", 8.2F);
            this.lblScaleFactor.Location = new System.Drawing.Point(8, 24);
            this.lblScaleFactor.Name = "lblScaleFactor";
            this.lblScaleFactor.Size = new System.Drawing.Size(76, 13);
            this.lblScaleFactor.TabIndex = 0;
            this.lblScaleFactor.Text = "Ölçek (Scale):";

            // 
            // tbScaleFactor
            // 
            this.tbScaleFactor.LargeChange = 2;
            this.tbScaleFactor.Location = new System.Drawing.Point(90, 18);
            this.tbScaleFactor.Maximum = 15;
            this.tbScaleFactor.Minimum = 5;
            this.tbScaleFactor.Name = "tbScaleFactor";
            this.tbScaleFactor.Size = new System.Drawing.Size(225, 45);
            this.tbScaleFactor.TabIndex = 1;
            this.tbScaleFactor.Value = 11;

            // 
            // lblScaleFactorVal
            // 
            this.lblScaleFactorVal.AutoSize = true;
            this.lblScaleFactorVal.Font = new System.Drawing.Font("Segoe UI", 8.2F, System.Drawing.FontStyle.Bold);
            this.lblScaleFactorVal.Location = new System.Drawing.Point(325, 24);
            this.lblScaleFactorVal.Name = "lblScaleFactorVal";
            this.lblScaleFactorVal.Size = new System.Drawing.Size(27, 13);
            this.lblScaleFactorVal.TabIndex = 2;
            this.lblScaleFactorVal.Text = "1.15";

            // 
            // lblNeighbors
            // 
            this.lblNeighbors.AutoSize = true;
            this.lblNeighbors.Font = new System.Drawing.Font("Segoe UI", 8.2F);
            this.lblNeighbors.Location = new System.Drawing.Point(8, 62);
            this.lblNeighbors.Name = "lblNeighbors";
            this.lblNeighbors.Size = new System.Drawing.Size(84, 13);
            this.lblNeighbors.TabIndex = 3;
            this.lblNeighbors.Text = "Komşu (Kriter):";

            // 
            // tbNeighbors
            // 
            this.tbNeighbors.Location = new System.Drawing.Point(90, 56);
            this.tbNeighbors.Maximum = 10;
            this.tbNeighbors.Minimum = 2;
            this.tbNeighbors.Name = "tbNeighbors";
            this.tbNeighbors.Size = new System.Drawing.Size(225, 45);
            this.tbNeighbors.TabIndex = 4;
            this.tbNeighbors.Value = 4;

            // 
            // lblNeighborsVal
            // 
            this.lblNeighborsVal.AutoSize = true;
            this.lblNeighborsVal.Font = new System.Drawing.Font("Segoe UI", 8.2F, System.Drawing.FontStyle.Bold);
            this.lblNeighborsVal.Location = new System.Drawing.Point(325, 62);
            this.lblNeighborsVal.Name = "lblNeighborsVal";
            this.lblNeighborsVal.Size = new System.Drawing.Size(13, 13);
            this.lblNeighborsVal.TabIndex = 5;
            this.lblNeighborsVal.Text = "4";

            // 
            // grpDetections
            // 
            this.grpDetections.Controls.Add(this.dgvDetections);
            this.grpDetections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetections.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.grpDetections.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpDetections.Location = new System.Drawing.Point(0, 330);
            this.grpDetections.Name = "grpDetections";
            this.grpDetections.Size = new System.Drawing.Size(382, 266);
            this.grpDetections.TabIndex = 3;
            this.grpDetections.TabStop = false;
            this.grpDetections.Text = "🎯 Canlı Algılama ve Tanıma Geçmişi (0)";

            // 
            // dgvDetections
            // 
            this.dgvDetections.AllowUserToAddRows = false;
            this.dgvDetections.AllowUserToDeleteRows = false;
            this.dgvDetections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetections.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetections.Location = new System.Drawing.Point(3, 19);
            this.dgvDetections.Name = "dgvDetections";
            this.dgvDetections.ReadOnly = true;
            this.dgvDetections.RowHeadersVisible = false;
            this.dgvDetections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetections.Size = new System.Drawing.Size(376, 244);
            this.dgvDetections.TabIndex = 0;

            // 
            // pnlBottomActions
            // 
            this.pnlBottomActions.Controls.Add(this.btnExportLog);
            this.pnlBottomActions.Controls.Add(this.btnClearDetections);
            this.pnlBottomActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomActions.Location = new System.Drawing.Point(0, 596);
            this.pnlBottomActions.Name = "pnlBottomActions";
            this.pnlBottomActions.Size = new System.Drawing.Size(382, 36);
            this.pnlBottomActions.TabIndex = 4;

            // 
            // btnExportLog
            // 
            this.btnExportLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnExportLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportLog.FlatAppearance.BorderSize = 0;
            this.btnExportLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportLog.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnExportLog.ForeColor = System.Drawing.Color.White;
            this.btnExportLog.Location = new System.Drawing.Point(2, 4);
            this.btnExportLog.Name = "btnExportLog";
            this.btnExportLog.Size = new System.Drawing.Size(220, 28);
            this.btnExportLog.TabIndex = 0;
            this.btnExportLog.Text = "💾 Tanıma Raporu Kaydet (.txt)";
            this.btnExportLog.UseVisualStyleBackColor = false;

            // 
            // btnClearDetections
            // 
            this.btnClearDetections.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnClearDetections.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearDetections.FlatAppearance.BorderSize = 0;
            this.btnClearDetections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearDetections.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnClearDetections.ForeColor = System.Drawing.Color.White;
            this.btnClearDetections.Location = new System.Drawing.Point(230, 4);
            this.btnClearDetections.Name = "btnClearDetections";
            this.btnClearDetections.Size = new System.Drawing.Size(148, 28);
            this.btnClearDetections.TabIndex = 1;
            this.btnClearDetections.Text = "🗑️ Tabloyu Temizle";
            this.btnClearDetections.UseVisualStyleBackColor = false;

            // 
            // pnlVideoContainer
            // 
            this.pnlVideoContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlVideoContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVideoContainer.Controls.Add(this.pbVideoFeed);
            this.pnlVideoContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVideoContainer.Location = new System.Drawing.Point(0, 60);
            this.pnlVideoContainer.Name = "pnlVideoContainer";
            this.pnlVideoContainer.Size = new System.Drawing.Size(770, 640);
            this.pnlVideoContainer.Padding = new System.Windows.Forms.Padding(6);
            this.pnlVideoContainer.TabIndex = 1;

            // 
            // pbVideoFeed
            // 
            this.pbVideoFeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pbVideoFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbVideoFeed.Location = new System.Drawing.Point(6, 6);
            this.pbVideoFeed.Name = "pbVideoFeed";
            this.pbVideoFeed.Size = new System.Drawing.Size(756, 626);
            this.pbVideoFeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVideoFeed.TabIndex = 0;
            this.pbVideoFeed.TabStop = false;

            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFooterStatus,
            this.lblFooterStudent});
            this.statusStrip.Location = new System.Drawing.Point(0, 700);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1160, 24);
            this.statusStrip.TabIndex = 3;

            // 
            // lblFooterStatus
            // 
            this.lblFooterStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFooterStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblFooterStatus.Name = "lblFooterStatus";
            this.lblFooterStatus.Size = new System.Drawing.Size(320, 19);
            this.lblFooterStatus.Text = "Hazır | OpenCvSharp4, Göz, Gülümseme, TTS & Güvenlik Alarmı";

            // 
            // lblFooterStudent
            // 
            this.lblFooterStudent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFooterStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblFooterStudent.Name = "lblFooterStudent";
            this.lblFooterStudent.Size = new System.Drawing.Size(825, 19);
            this.lblFooterStudent.Spring = true;
            this.lblFooterStudent.Text = "Öğrenci: İsmail Özdemir (250541109) - YMH 219";
            this.lblFooterStudent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 724);
            this.Controls.Add(this.pnlVideoContainer);
            this.Controls.Add(this.pnlSideDashboard);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlTopBar);
            this.MinimumSize = new System.Drawing.Size(1080, 680);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YMH219 - OpenCV ile Yüz Algılama ve Tanıma (FaceRecognition)";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlVideoContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoFeed)).EndInit();
            this.pnlSideDashboard.ResumeLayout(false);
            this.grpProfiles.ResumeLayout(false);
            this.grpFeatures.ResumeLayout(false);
            this.grpFeatures.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbScaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNeighbors)).EndInit();
            this.grpDetections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetections)).EndInit();
            this.pnlBottomActions.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
