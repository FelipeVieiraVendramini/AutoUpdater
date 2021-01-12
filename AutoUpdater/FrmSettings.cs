#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - FrmSettings.cs
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
using System.Management;
using System.Windows.Forms;
using AutoUpdaterCore;

#endregion

namespace AutoUpdater
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            var scope = new ManagementScope();
            var query = new ObjectQuery("SELECT * FROM CIM_VideoControllerResolution");

            using var searcher = new ManagementObjectSearcher(scope, query);
            var results = searcher.Get();

            foreach (var result in results)
            {
                string resolution = $"{result["HorizontalResolution"]}x{result["VerticalResolution"]}";

                if (int.Parse(result["HorizontalResolution"].ToString()) < 1024)
                    continue;

                if (!cmbScreenResolution.Items.Contains(resolution))
                    cmbScreenResolution.Items.Add(resolution);
            }

            if (cmbScreenResolution.Items.Count > 0)
                cmbScreenResolution.SelectedIndex = cmbScreenResolution.Items.Count - 1;

            IniFileName ini = new IniFileName(Environment.CurrentDirectory + @"\Config.ini");
            string szWidth = ini.GetEntryValue("GameResolution", "Width")?.ToString() ?? "1004";
            string szHeight = ini.GetEntryValue("GameResolution", "Height")?.ToString() ?? "708";
            string szFpsMode = ini.GetEntryValue("GameSetup", "FpsMode")?.ToString() ?? "2";
            string szInjectionDisable = ini.GetEntryValue("GameResolution", "NoWindowInjection")?.ToString() ?? "0";

            if (string.IsNullOrEmpty(szWidth) || !int.TryParse(szWidth, out _))
            {
                szWidth = "1004";
            }

            if (string.IsNullOrEmpty(szHeight) || !int.TryParse(szHeight, out _))
            {
                szHeight = "708";
            }

            if (string.IsNullOrEmpty(szFpsMode) || !int.TryParse(szFpsMode, out _))
            {
                szFpsMode = "1";
            }

            if (string.IsNullOrEmpty(szInjectionDisable) || !int.TryParse(szInjectionDisable, out _))
            {
                szInjectionDisable = "0";
            }

            int width = int.Parse(szWidth) + 20;
            int height = int.Parse(szHeight) + 60;

            for (int i = 0; i < cmbScreenResolution.Items.Count; i++)
            {
                if (cmbScreenResolution.Items[i].ToString().Equals($"{width}x{height}"))
                    cmbScreenResolution.SelectedIndex = i;
            }

            int fpsMode = int.Parse(szFpsMode);
            switch (fpsMode)
            {
                case 1:
                    radio60Fps.Checked = true;
                    break;
                case 2:
                    radioUnlockedFps.Checked = true;
                    break;
                default:
                    radioNormalFps.Checked = true;
                    break;
            }

            chkNoInjection.Checked = int.Parse(szInjectionDisable) != 0;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            IniFileName ini = new IniFileName(Environment.CurrentDirectory + @"\ini\GameSetUp.ini");
            ini.SetValue("ScreenMode", "ScreenModeRecord", 2);

            string selectedResolution = cmbScreenResolution.Items[cmbScreenResolution.SelectedIndex].ToString();
            int width = Math.Max(1024, int.Parse(selectedResolution.Split('x')[0]) - 20);
            int height = Math.Max(768, int.Parse(selectedResolution.Split('x')[1]) - 60);
            bool noInjection = chkNoInjection.Checked;
            int fpsMode = 0;
            if (radioNormalFps.Checked)
            {
                fpsMode = 0;
            }
            else if (radio60Fps.Checked)
            {
                fpsMode = 1;
            }
            else if (radioUnlockedFps.Checked)
            {
                fpsMode = 2;
            }

            Kernel.SetClientConfiguration(width, height, noInjection, fpsMode);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoInjection.Checked)
            {
                for (int i = 0; i < cmbScreenResolution.Items.Count; i++)
                {
                    if (cmbScreenResolution.Items[i].Equals("1024x768"))
                    {
                        cmbScreenResolution.SelectedIndex = i;
                        break;
                    }
                }

                cmbScreenResolution.Enabled = false;
            }
            else cmbScreenResolution.Enabled = true;
        }
    }
}