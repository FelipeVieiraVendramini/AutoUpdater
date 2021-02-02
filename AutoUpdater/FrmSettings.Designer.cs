
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
            this.radioFpsCustom = new System.Windows.Forms.RadioButton();
            this.numCustomFps = new System.Windows.Forms.NumericUpDown();
            this.chkCustomRes = new System.Windows.Forms.CheckBox();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numCustomFps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
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
            this.radioNormalFps.Location = new System.Drawing.Point(124, 24);
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
            this.radio60Fps.Location = new System.Drawing.Point(124, 47);
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
            this.radioUnlockedFps.Location = new System.Drawing.Point(124, 70);
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
            this.cmbScreenResolution.Location = new System.Drawing.Point(124, 117);
            this.cmbScreenResolution.Name = "cmbScreenResolution";
            this.cmbScreenResolution.Size = new System.Drawing.Size(191, 21);
            this.cmbScreenResolution.TabIndex = 9;
            // 
            // lblScreenSize
            // 
            this.lblScreenSize.AutoSize = true;
            this.lblScreenSize.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenSize.Location = new System.Drawing.Point(23, 120);
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
            // radioFpsCustom
            // 
            this.radioFpsCustom.AutoSize = true;
            this.radioFpsCustom.BackColor = System.Drawing.Color.Transparent;
            this.radioFpsCustom.Location = new System.Drawing.Point(124, 93);
            this.radioFpsCustom.Name = "radioFpsCustom";
            this.radioFpsCustom.Size = new System.Drawing.Size(14, 13);
            this.radioFpsCustom.TabIndex = 12;
            this.radioFpsCustom.UseVisualStyleBackColor = false;
            this.radioFpsCustom.CheckedChanged += new System.EventHandler(this.radioFpsCustom_CheckedChanged);
            // 
            // numCustomFps
            // 
            this.numCustomFps.Enabled = false;
            this.numCustomFps.Location = new System.Drawing.Point(144, 89);
            this.numCustomFps.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCustomFps.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numCustomFps.Name = "numCustomFps";
            this.numCustomFps.Size = new System.Drawing.Size(76, 20);
            this.numCustomFps.TabIndex = 15;
            this.numCustomFps.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // chkCustomRes
            // 
            this.chkCustomRes.AutoSize = true;
            this.chkCustomRes.BackColor = System.Drawing.Color.Transparent;
            this.chkCustomRes.Location = new System.Drawing.Point(26, 144);
            this.chkCustomRes.Name = "chkCustomRes";
            this.chkCustomRes.Size = new System.Drawing.Size(92, 17);
            this.chkCustomRes.TabIndex = 16;
            this.chkCustomRes.Text = "Personalizada";
            this.chkCustomRes.UseVisualStyleBackColor = false;
            this.chkCustomRes.CheckedChanged += new System.EventHandler(this.chkCustomRes_CheckedChanged);
            // 
            // numWidth
            // 
            this.numWidth.Enabled = false;
            this.numWidth.Location = new System.Drawing.Point(124, 141);
            this.numWidth.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(86, 20);
            this.numWidth.TabIndex = 17;
            this.numWidth.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // numHeight
            // 
            this.numHeight.Enabled = false;
            this.numHeight.Location = new System.Drawing.Point(229, 141);
            this.numHeight.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(86, 20);
            this.numHeight.TabIndex = 17;
            this.numHeight.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AutoUpdater.Properties.Resources.Hint;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 207);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.chkCustomRes);
            this.Controls.Add(this.numCustomFps);
            this.Controls.Add(this.lblFramesPerSecond);
            this.Controls.Add(this.lblScreenSize);
            this.Controls.Add(this.radioNormalFps);
            this.Controls.Add(this.radio60Fps);
            this.Controls.Add(this.radioFpsCustom);
            this.Controls.Add(this.radioUnlockedFps);
            this.Controls.Add(this.cmbScreenResolution);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmSettings";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numCustomFps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
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
        private System.Windows.Forms.RadioButton radioFpsCustom;
        private System.Windows.Forms.NumericUpDown numCustomFps;
        private System.Windows.Forms.CheckBox chkCustomRes;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeight;
    }
}