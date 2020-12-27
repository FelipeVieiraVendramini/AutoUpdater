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

            if (string.IsNullOrEmpty(szWidth))
            {
                szWidth = "1004";
            }

            if (string.IsNullOrEmpty(szHeight))
            {
                szHeight = "708";
            }

            if (string.IsNullOrEmpty(szFpsMode))
            {
                szFpsMode = "1";
            }

            if (string.IsNullOrEmpty(szInjectionDisable))
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
            int windowInjection = chkNoInjection.Checked ? 1 : 0;

            ini = new IniFileName(Environment.CurrentDirectory + @"\Config.ini");
            ini.SetValue("GameResolution", "Width", width);
            ini.SetValue("GameResolution", "Height", height);
            ini.SetValue("GameResolution", "NoWindowInjection", windowInjection.ToString());

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

            ini.SetValue("GameSetup", "FpsMode", fpsMode);
            
            ini = new IniFileName(Environment.CurrentDirectory + @"\ini\GUI.ini");
            // Main interface
            ini.SetValue("0-130", "x", (width - 1024) / 2);
            ini.SetValue("0-130", "y", height - 141);

            // Chat panel
            ini.SetValue("0-145", "x", ((width - 1024) / 2) + 82);
            ini.SetValue("0-145", "y", height - 71);

            // Chat Player Names
            ini.SetValue("0-148", "x", ((width - 1024) / 2) + 82 + 35);
            ini.SetValue("0-148", "y", height - 65 - 300);

            // Chat Channel
            ini.SetValue("0-174", "x", ((width - 1024) / 2) + 82 + 172);
            ini.SetValue("0-174", "y", height - 65 - 205);

            // Path Finding Button
            ini.SetValue("0-304", "x", width - 110);

            // Path Finding GUI
            ini.SetValue("0-303", "x", width - 530);

            // Exit Game
            ini.SetValue("0-158", "x", (width - 288) / 2);
            ini.SetValue("0-158", "y", (height - 120) / 2);

            // Options
            ini.SetValue("0-138", "x", (width - 370) / 2);
            ini.SetValue("0-138", "y", (height - 350) / 2);

            // Team
            ini.SetValue("0-141", "x", (width - 290) / 2);
            ini.SetValue("0-268", "x", (width - 58) / 2);
            
            //VIP Button
            ini.SetValue("0-339", "x", ((width - 1024) / 2) + 82 + 205);
            ini.SetValue("0-339", "y", height - 115);

            // My Talisman UI
            ini.SetValue("0-345", "x", 9999);
            // Target Talisman UI
            ini.SetValue("0-346", "x", 9999);

            // Shopping Mall
            ini.SetValue("0-289", "x", ((width - 1024) / 2) + 82 + 50);
            ini.SetValue("0-289", "y", height - 115);

            // Mentor
            ini.SetValue("0-325", "x", ((width - 1024) / 2) + 82 + 85);
            ini.SetValue("0-325", "y", height - 130);

            // Item Lock
            ini.SetValue("0-328", "x", ((width - 1024) / 2) + 82 + 145);
            ini.SetValue("0-328", "y", height - 110);

            // Whisper Player Avatars
            ini.SetValue("0-3", "x", ((width - 1024) / 2) + 82 + 400);
            ini.SetValue("0-3", "y", height - 115);

            // PKModes
            ini.SetValue("0-191", "x", ((width - 1024) / 2) + 82 + 500);
            ini.SetValue("0-191", "y", height - 100);

            // Map Mini
            ini.SetValue("0-1199", "x", width - 40);

            // Map Full
            ini.SetValue("0-1200", "x", width - 20);

            // Actions
            ini.SetValue("0-140", "x", ((width - 1024) / 2) + 250);
            ini.SetValue("0-140", "y", height - 180);

            ini = new IniFileName(Environment.CurrentDirectory + @"\ini\info.ini");
            // Exp
            ini.SetValue("ExpShowPos", "Exp_XPos", (width / 2) - 150);
            ini.SetValue("ExpShowPos", "Exp_YPos", height - 90);

            // AddExp
            ini.SetValue("ExpShowPos", "AddExp_XPos", (width / 2) - 150 + 90);
            ini.SetValue("ExpShowPos", "AddExp_YPos", height - 90);

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