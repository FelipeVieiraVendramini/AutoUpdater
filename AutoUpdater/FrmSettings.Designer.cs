
namespace AutoUpdater
{
    partial class FrmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.radioNormalFps = new System.Windows.Forms.RadioButton();
            this.radio60Fps = new System.Windows.Forms.RadioButton();
            this.radioUnlockedFps = new System.Windows.Forms.RadioButton();
            this.cmbScreenResolution = new System.Windows.Forms.ComboBox();
            this.lblScreenSize = new System.Windows.Forms.Label();
            this.lblFramesPerSecond = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(301, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(220, 164);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "&Gravar";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // radioNormalFps
            // 
            this.radioNormalFps.AutoSize = true;
            this.radioNormalFps.BackColor = System.Drawing.Color.Transparent;
            this.radioNormalFps.Location = new System.Drawing.Point(111, 24);
            this.radioNormalFps.Name = "radioNormalFps";
            this.radioNormalFps.Size = new System.Drawing.Size(81, 17);
            this.radioNormalFps.TabIndex = 10;
            this.radioNormalFps.Text = "FPS Normal";
            this.radioNormalFps.UseVisualStyleBackColor = false;
            // 
            // radio60Fps
            // 
            this.radio60Fps.AutoSize = true;
            this.radio60Fps.BackColor = System.Drawing.Color.Transparent;
            this.radio60Fps.Checked = true;
            this.radio60Fps.Location = new System.Drawing.Point(111, 47);
            this.radio60Fps.Name = "radio60Fps";
            this.radio60Fps.Size = new System.Drawing.Size(66, 17);
            this.radio60Fps.TabIndex = 11;
            this.radio60Fps.TabStop = true;
            this.radio60Fps.Text = "120 FPS";
            this.radio60Fps.UseVisualStyleBackColor = false;
            // 
            // radioUnlockedFps
            // 
            this.radioUnlockedFps.AutoSize = true;
            this.radioUnlockedFps.BackColor = System.Drawing.Color.Transparent;
            this.radioUnlockedFps.Location = new System.Drawing.Point(111, 70);
            this.radioUnlockedFps.Name = "radioUnlockedFps";
            this.radioUnlockedFps.Size = new System.Drawing.Size(86, 17);
            this.radioUnlockedFps.TabIndex = 12;
            this.radioUnlockedFps.Text = "FPS Ilimitado";
            this.radioUnlockedFps.UseVisualStyleBackColor = false;
            // 
            // cmbScreenResolution
            // 
            this.cmbScreenResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScreenResolution.FormattingEnabled = true;
            this.cmbScreenResolution.Location = new System.Drawing.Point(111, 135);
            this.cmbScreenResolution.Name = "cmbScreenResolution";
            this.cmbScreenResolution.Size = new System.Drawing.Size(173, 21);
            this.cmbScreenResolution.TabIndex = 9;
            // 
            // lblScreenSize
            // 
            this.lblScreenSize.AutoSize = true;
            this.lblScreenSize.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenSize.Location = new System.Drawing.Point(23, 140);
            this.lblScreenSize.Name = "lblScreenSize";
            this.lblScreenSize.Size = new System.Drawing.Size(58, 13);
            this.lblScreenSize.TabIndex = 13;
            this.lblScreenSize.Text = "Resolução";
            // 
            // lblFramesPerSecond
            // 
            this.lblFramesPerSecond.AutoSize = true;
            this.lblFramesPerSecond.BackColor = System.Drawing.Color.Transparent;
            this.lblFramesPerSecond.Location = new System.Drawing.Point(23, 28);
            this.lblFramesPerSecond.Name = "lblFramesPerSecond";
            this.lblFramesPerSecond.Size = new System.Drawing.Size(27, 13);
            this.lblFramesPerSecond.TabIndex = 14;
            this.lblFramesPerSecond.Text = "FPS";
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AutoUpdater.Properties.Resources.Hint;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 207);
            this.Controls.Add(this.lblFramesPerSecond);
            this.Controls.Add(this.lblScreenSize);
            this.Controls.Add(this.radioNormalFps);
            this.Controls.Add(this.radio60Fps);
            this.Controls.Add(this.radioUnlockedFps);
            this.Controls.Add(this.cmbScreenResolution);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmSettings";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.RadioButton radioNormalFps;
        private System.Windows.Forms.RadioButton radio60Fps;
        private System.Windows.Forms.RadioButton radioUnlockedFps;
        private System.Windows.Forms.ComboBox cmbScreenResolution;
        private System.Windows.Forms.Label lblScreenSize;
        private System.Windows.Forms.Label lblFramesPerSecond;
    }
}