namespace AutoUpdater
{
    partial class FrmTermsOfPrivacy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTermsOfPrivacy));
            this.lblTermsOfServiceDesc = new System.Windows.Forms.LinkLabel();
            this.lblTermsOfServiceTitle = new System.Windows.Forms.Label();
            this.chkPrivacy = new System.Windows.Forms.CheckBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTermsOfServiceDesc
            // 
            this.lblTermsOfServiceDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblTermsOfServiceDesc.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblTermsOfServiceDesc.Location = new System.Drawing.Point(24, 57);
            this.lblTermsOfServiceDesc.Name = "lblTermsOfServiceDesc";
            this.lblTermsOfServiceDesc.Size = new System.Drawing.Size(346, 97);
            this.lblTermsOfServiceDesc.TabIndex = 0;
            this.lblTermsOfServiceDesc.Text = resources.GetString("lblTermsOfServiceDesc.Text");
            this.lblTermsOfServiceDesc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblTermsOfServiceDesc_LinkClicked);
            // 
            // lblTermsOfServiceTitle
            // 
            this.lblTermsOfServiceTitle.AutoSize = true;
            this.lblTermsOfServiceTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTermsOfServiceTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTermsOfServiceTitle.Location = new System.Drawing.Point(23, 22);
            this.lblTermsOfServiceTitle.Name = "lblTermsOfServiceTitle";
            this.lblTermsOfServiceTitle.Size = new System.Drawing.Size(184, 24);
            this.lblTermsOfServiceTitle.TabIndex = 1;
            this.lblTermsOfServiceTitle.Text = "Termos de serviço";
            // 
            // chkPrivacy
            // 
            this.chkPrivacy.AutoSize = true;
            this.chkPrivacy.BackColor = System.Drawing.Color.Transparent;
            this.chkPrivacy.Location = new System.Drawing.Point(27, 163);
            this.chkPrivacy.Name = "chkPrivacy";
            this.chkPrivacy.Size = new System.Drawing.Size(201, 17);
            this.chkPrivacy.TabIndex = 2;
            this.chkPrivacy.Text = "Li e aceito os Termos de Privacidade";
            this.chkPrivacy.UseVisualStyleBackColor = false;
            this.chkPrivacy.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Enabled = false;
            this.btnAccept.Location = new System.Drawing.Point(240, 159);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(62, 23);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.Text = "&Aceitar";
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(308, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmTermsOfPrivacy
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AutoUpdater.Properties.Resources.Hint;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(397, 207);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.chkPrivacy);
            this.Controls.Add(this.lblTermsOfServiceTitle);
            this.Controls.Add(this.lblTermsOfServiceDesc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmTermsOfPrivacy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmTermsOfPrivacy";
            this.Load += new System.EventHandler(this.FrmTermsOfPrivacy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lblTermsOfServiceDesc;
        private System.Windows.Forms.Label lblTermsOfServiceTitle;
        private System.Windows.Forms.CheckBox chkPrivacy;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
    }
}