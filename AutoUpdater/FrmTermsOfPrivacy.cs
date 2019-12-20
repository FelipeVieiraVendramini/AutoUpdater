#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - FrmTermsOfPrivacy.cs
// 
// Description: <Write a description for this file>
// 
// Colaborators who worked in this file:
// Felipe Vieira Vendramini
// 
// Developed by:
// Felipe Vieira Vendramini <service@ftwmasters.com.br>
// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

#region References

using System;
using System.Windows.Forms;

#endregion

namespace AutoUpdater
{
    public partial class FrmTermsOfPrivacy : Form
    {
        private string m_szLink = "";

        public FrmTermsOfPrivacy(string link)
        {
            InitializeComponent();

            m_szLink = link;
        }

        private void FrmTermsOfPrivacy_Load(object sender, EventArgs e)
        {
            string title = LanguageManager.GetString("StrTermsOfPrivacyTitle");
            string desc = LanguageManager.GetString("StrTermsOfPrivacyDesc");
            int start = desc.IndexOf("%privacy_terms%");
            desc = desc.Replace("%privacy_terms%", title);

            lblTermsOfServiceTitle.Text = title;
            lblTermsOfServiceDesc.Text = desc;
            lblTermsOfServiceDesc.LinkArea = new LinkArea(start, title.Length);
        }

        private void lblTermsOfServiceDesc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FrmWebBrowser(m_szLink).ShowDialog(this);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            btnAccept.Enabled = chkPrivacy.Checked;
        }
    }
}