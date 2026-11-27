namespace FaceRecognition.Forms
{
    partial class TrainFaceDialog
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtPersonName;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.PictureBox pbFacePreview;
        private System.Windows.Forms.Label lblPreviewInfo;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnCancel;

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
            this.lblName = new System.Windows.Forms.Label();
            this.txtPersonName = new System.Windows.Forms.TextBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.pbFacePreview = new System.Windows.Forms.PictureBox();
            this.lblPreviewInfo = new System.Windows.Forms.Label();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbFacePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(126, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Kişi Adı ve Soyadı:";
            // 
            // txtPersonName
            // 
            this.txtPersonName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPersonName.Location = new System.Drawing.Point(23, 44);
            this.txtPersonName.Name = "txtPersonName";
            this.txtPersonName.Size = new System.Drawing.Size(320, 25);
            this.txtPersonName.TabIndex = 1;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnSelectImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectImage.FlatAppearance.BorderSize = 0;
            this.btnSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSelectImage.ForeColor = System.Drawing.Color.White;
            this.btnSelectImage.Location = new System.Drawing.Point(23, 80);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(320, 32);
            this.btnSelectImage.TabIndex = 2;
            this.btnSelectImage.Text = "📷 Fotoğraf Seç ve Yüzü Kırp";
            this.btnSelectImage.UseVisualStyleBackColor = false;
            // 
            // pbFacePreview
            // 
            this.pbFacePreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pbFacePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFacePreview.Location = new System.Drawing.Point(103, 125);
            this.pbFacePreview.Name = "pbFacePreview";
            this.pbFacePreview.Size = new System.Drawing.Size(150, 150);
            this.pbFacePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFacePreview.TabIndex = 3;
            this.pbFacePreview.TabStop = false;
            // 
            // lblPreviewInfo
            // 
            this.lblPreviewInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPreviewInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblPreviewInfo.Location = new System.Drawing.Point(23, 282);
            this.lblPreviewInfo.Name = "lblPreviewInfo";
            this.lblPreviewInfo.Size = new System.Drawing.Size(320, 20);
            this.lblPreviewInfo.TabIndex = 4;
            this.lblPreviewInfo.Text = "Henüz yüz fotoğrafı seçilmedi.";
            this.lblPreviewInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnSaveProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveProfile.Enabled = false;
            this.btnSaveProfile.FlatAppearance.BorderSize = 0;
            this.btnSaveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProfile.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSaveProfile.ForeColor = System.Drawing.Color.White;
            this.btnSaveProfile.Location = new System.Drawing.Point(23, 315);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(190, 36);
            this.btnSaveProfile.TabIndex = 5;
            this.btnSaveProfile.Text = "💾 Modele Eğit ve Kaydet";
            this.btnSaveProfile.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(223, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 36);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // TrainFaceDialog
            // 
            this.AcceptButton = this.btnSaveProfile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(368, 370);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveProfile);
            this.Controls.Add(this.lblPreviewInfo);
            this.Controls.Add(this.pbFacePreview);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.txtPersonName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrainFaceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "➕ Yeni Yüz Eğit ve Modele Ekle";
            ((System.ComponentModel.ISupportInitialize)(this.pbFacePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
