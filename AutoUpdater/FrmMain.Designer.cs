﻿namespace AutoUpdater
{
    partial class FrmMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnPlayHigh = new System.Windows.Forms.Button();
            this.btnPlayLow = new System.Windows.Forms.Button();
            this.lblGameVersion = new System.Windows.Forms.Label();
            this.lblUpdaterVersion = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnRanking = new System.Windows.Forms.Button();
            this.btnSite = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblCenterStatus = new System.Windows.Forms.Label();
            this.panelProgressbar = new System.Windows.Forms.Panel();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.panelStatus.SuspendLayout();
            this.panelProgressbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlayHigh
            // 
            this.btnPlayHigh.BackColor = System.Drawing.Color.Transparent;
            this.btnPlayHigh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlayHigh.Enabled = false;
            this.btnPlayHigh.FlatAppearance.BorderSize = 0;
            this.btnPlayHigh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayHigh.Image = ((System.Drawing.Image)(resources.GetObject("btnPlayHigh.Image")));
            this.btnPlayHigh.Location = new System.Drawing.Point(596, 426);
            this.btnPlayHigh.Name = "btnPlayHigh";
            this.btnPlayHigh.Size = new System.Drawing.Size(210, 83);
            this.btnPlayHigh.TabIndex = 0;
            this.btnPlayHigh.UseVisualStyleBackColor = false;
            this.btnPlayHigh.Click += new System.EventHandler(this.btnPlayHigh_Click);
            this.btnPlayHigh.MouseEnter += new System.EventHandler(this.btnPlayHigh_MouseEnter);
            this.btnPlayHigh.MouseLeave += new System.EventHandler(this.btnPlayHigh_MouseLeave);
            // 
            // btnPlayLow
            // 
            this.btnPlayLow.BackColor = System.Drawing.Color.Transparent;
            this.btnPlayLow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlayLow.Enabled = false;
            this.btnPlayLow.FlatAppearance.BorderSize = 0;
            this.btnPlayLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayLow.Image = ((System.Drawing.Image)(resources.GetObject("btnPlayLow.Image")));
            this.btnPlayLow.Location = new System.Drawing.Point(588, 498);
            this.btnPlayLow.Name = "btnPlayLow";
            this.btnPlayLow.Size = new System.Drawing.Size(210, 90);
            this.btnPlayLow.TabIndex = 0;
            this.btnPlayLow.UseVisualStyleBackColor = false;
            this.btnPlayLow.Click += new System.EventHandler(this.btnPlayLow_Click);
            this.btnPlayLow.MouseEnter += new System.EventHandler(this.btnPlayLow_MouseEnter);
            this.btnPlayLow.MouseLeave += new System.EventHandler(this.btnPlayLow_MouseLeave);
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.AutoSize = true;
            this.lblGameVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblGameVersion.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblGameVersion.Location = new System.Drawing.Point(603, 27);
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(52, 13);
            this.lblGameVersion.TabIndex = 1;
            this.lblGameVersion.Text = "lblVersion";
            // 
            // lblUpdaterVersion
            // 
            this.lblUpdaterVersion.AutoSize = true;
            this.lblUpdaterVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdaterVersion.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblUpdaterVersion.Location = new System.Drawing.Point(309, 78);
            this.lblUpdaterVersion.Name = "lblUpdaterVersion";
            this.lblUpdaterVersion.Size = new System.Drawing.Size(52, 13);
            this.lblUpdaterVersion.TabIndex = 1;
            this.lblUpdaterVersion.Text = "lblVersion";
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Image = ((System.Drawing.Image)(resources.GetObject("btnRegister.Image")));
            this.btnRegister.Location = new System.Drawing.Point(36, 528);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(108, 30);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.btnRegister.MouseEnter += new System.EventHandler(this.btnRegister_MouseEnter);
            this.btnRegister.MouseLeave += new System.EventHandler(this.btnRegister_MouseLeave);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnDownload.FlatAppearance.BorderSize = 0;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Location = new System.Drawing.Point(147, 528);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(108, 30);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            this.btnDownload.MouseEnter += new System.EventHandler(this.btnDownload_MouseEnter);
            this.btnDownload.MouseLeave += new System.EventHandler(this.btnDownload_MouseLeave);
            // 
            // btnRanking
            // 
            this.btnRanking.BackColor = System.Drawing.Color.Transparent;
            this.btnRanking.FlatAppearance.BorderSize = 0;
            this.btnRanking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRanking.Image = ((System.Drawing.Image)(resources.GetObject("btnRanking.Image")));
            this.btnRanking.Location = new System.Drawing.Point(258, 528);
            this.btnRanking.Name = "btnRanking";
            this.btnRanking.Size = new System.Drawing.Size(108, 30);
            this.btnRanking.TabIndex = 2;
            this.btnRanking.UseVisualStyleBackColor = false;
            this.btnRanking.Click += new System.EventHandler(this.btnRanking_Click);
            this.btnRanking.MouseEnter += new System.EventHandler(this.btnRanking_MouseEnter);
            this.btnRanking.MouseLeave += new System.EventHandler(this.btnRanking_MouseLeave);
            // 
            // btnSite
            // 
            this.btnSite.BackColor = System.Drawing.Color.Transparent;
            this.btnSite.FlatAppearance.BorderSize = 0;
            this.btnSite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSite.Image = ((System.Drawing.Image)(resources.GetObject("btnSite.Image")));
            this.btnSite.Location = new System.Drawing.Point(369, 528);
            this.btnSite.Name = "btnSite";
            this.btnSite.Size = new System.Drawing.Size(108, 30);
            this.btnSite.TabIndex = 2;
            this.btnSite.UseVisualStyleBackColor = false;
            this.btnSite.Click += new System.EventHandler(this.btnSite_Click);
            this.btnSite.MouseEnter += new System.EventHandler(this.btnSite_MouseEnter);
            this.btnSite.MouseLeave += new System.EventHandler(this.btnSite_MouseLeave);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Enabled = false;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(480, 528);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(108, 30);
            this.btnExit.TabIndex = 2;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.MouseEnter += new System.EventHandler(this.btnExit_MouseEnter);
            this.btnExit.MouseLeave += new System.EventHandler(this.btnExit_MouseLeave);
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.Transparent;
            this.panelStatus.Controls.Add(this.lblCenterStatus);
            this.panelStatus.Location = new System.Drawing.Point(36, 458);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(552, 51);
            this.panelStatus.TabIndex = 3;
            // 
            // lblCenterStatus
            // 
            this.lblCenterStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCenterStatus.ForeColor = System.Drawing.Color.White;
            this.lblCenterStatus.Location = new System.Drawing.Point(0, 0);
            this.lblCenterStatus.Name = "lblCenterStatus";
            this.lblCenterStatus.Size = new System.Drawing.Size(552, 51);
            this.lblCenterStatus.TabIndex = 0;
            this.lblCenterStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelProgressbar
            // 
            this.panelProgressbar.BackColor = System.Drawing.Color.Transparent;
            this.panelProgressbar.BackgroundImage = global::AutoUpdater.Properties.Resources.IMAGE_LOADINGBACK;
            this.panelProgressbar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelProgressbar.Controls.Add(this.pbDownload);
            this.panelProgressbar.Location = new System.Drawing.Point(36, 458);
            this.panelProgressbar.Name = "panelProgressbar";
            this.panelProgressbar.Size = new System.Drawing.Size(552, 51);
            this.panelProgressbar.TabIndex = 0;
            this.panelProgressbar.Visible = false;
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(14, 7);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(520, 35);
            this.pbDownload.TabIndex = 0;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AutoUpdater.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSite);
            this.Controls.Add(this.btnRanking);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.lblUpdaterVersion);
            this.Controls.Add(this.lblGameVersion);
            this.Controls.Add(this.btnPlayLow);
            this.Controls.Add(this.btnPlayHigh);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelProgressbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Updater";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.panelStatus.ResumeLayout(false);
            this.panelProgressbar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlayHigh;
        private System.Windows.Forms.Button btnPlayLow;
        private System.Windows.Forms.Label lblGameVersion;
        private System.Windows.Forms.Label lblUpdaterVersion;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnRanking;
        private System.Windows.Forms.Button btnSite;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Panel panelProgressbar;
        private System.Windows.Forms.Label lblCenterStatus;
        private System.Windows.Forms.ProgressBar pbDownload;
    }
}
