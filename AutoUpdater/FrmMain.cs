﻿#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - FrmMain.cs
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

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using AutoUpdater.Properties;
using AutoUpdater.Sockets;
using AutoUpdaterCore;
using AutoUpdaterCore.Security;
using AutoUpdaterCore.Sockets;

namespace AutoUpdater
{
    public partial class FrmMain : Form
    {
        private byte[] _MODULUS =
        {
            0xDA, 0xA5, 0x85, 0x5D, 0x2E, 0xB9, 0x70, 0xB7, 0x96, 0x2C, 0x45, 0xFA, 0x4A, 0xBA, 0x26, 0x6D, 0x9D, 0x8E,
            0x7E, 0x58, 0xF2, 0xFF, 0x8E, 0x23, 0xA8, 0x8E, 0x35, 0x9A, 0x51, 0x44, 0xBC, 0xF5, 0x8D, 0x0A, 0x49, 0x27,
            0xF7, 0x25, 0x64, 0xA6, 0x74, 0x7C, 0x84, 0x2B, 0x48, 0x4E, 0xFD, 0xBC, 0x9F, 0x4D, 0xE1, 0xCD, 0x7E, 0xF4,
            0xA9, 0x91, 0xE5, 0x30, 0xF5, 0xA7, 0x0D, 0xF0, 0x6B, 0x25, 0x9D, 0xA6, 0xC4, 0x65, 0x5C, 0xE4, 0x63, 0xDD,
            0xDE, 0x73, 0x63, 0xEB, 0x8E, 0x89, 0x33, 0x95, 0x86, 0x77, 0x14, 0x50, 0x3A, 0x77, 0xFF, 0x84, 0xF6, 0x3C,
            0xD8, 0x9B, 0xB5, 0x1D, 0xC5, 0x2E, 0x20, 0x22, 0x6C, 0x5D, 0x82, 0x8C, 0x8E, 0x78, 0x91, 0xCD, 0xBE, 0xC4,
            0x01, 0x91, 0xFD, 0x71, 0xBB, 0x0D, 0x4D, 0x6F, 0x39, 0x19, 0x46, 0x2B, 0x74, 0x17, 0x16, 0xAC, 0xC5, 0xB7,
            0x15, 0x61
        };

        private byte[] _EXPONENT = {0x01, 0x00, 0x01};

        private byte[] _P =
        {
            0xE9, 0xDE, 0x73, 0xB0, 0x70, 0xAA, 0x25, 0x95, 0xCF, 0x4E, 0xF4, 0x64, 0x8A, 0x9F, 0x74, 0x01, 0xCD, 0xCC,
            0xD2, 0x17, 0x3A, 0x01, 0xED, 0xFE, 0xF6, 0xAB, 0x45, 0x73, 0x30, 0x31, 0x54, 0x3A, 0x94, 0x88, 0xFA, 0x9E,
            0x9D, 0xDF, 0xF6, 0x07, 0xB3, 0xFA, 0xAD, 0xCD, 0x1D, 0x20, 0x85, 0xCE, 0xBA, 0x4E, 0x94, 0x3A, 0xA4, 0x25,
            0x30, 0x6C, 0xC1, 0x01, 0x17, 0xA9, 0xB3, 0xBC, 0xE4, 0x87
        };

        private byte[] _Q =
        {
            0xEF, 0x56, 0x4D, 0x4F, 0x90, 0x4C, 0x74, 0x0C, 0xE0, 0x47, 0x3F, 0x46, 0xB2, 0x28, 0x11, 0x35, 0x87, 0xD5,
            0xCF, 0x30, 0xE3, 0x4B, 0xCD, 0x69, 0xD6, 0x86, 0x14, 0xC2, 0x2E, 0x90, 0xB8, 0x82, 0x44, 0xF0, 0x18, 0xB5,
            0x1E, 0x0F, 0x19, 0x5C, 0xFF, 0x37, 0x09, 0x63, 0xA0, 0x94, 0x0F, 0x2A, 0xE8, 0x22, 0x4D, 0xA2, 0xE2, 0x39,
            0x7E, 0x14, 0x5B, 0xA2, 0x96, 0xE8, 0xDE, 0x45, 0x98, 0xD7
        };

        private byte[] _DP =
        {
            0xC5, 0x84, 0x41, 0x05, 0x05, 0x42, 0x4E, 0x58, 0x06, 0x2C, 0x52, 0xB0, 0x0F, 0x7B, 0xB8, 0x08, 0x45, 0xFD,
            0xF4, 0x79, 0xF5, 0x5D, 0xE7, 0xD9, 0x6C, 0x0F, 0x1E, 0xAA, 0xB8, 0x4B, 0x11, 0x2E, 0x20, 0x80, 0xC7, 0xD9,
            0xEA, 0xD0, 0x5A, 0x04, 0x0B, 0x77, 0x6E, 0x0E, 0x4B, 0xC9, 0x49, 0xF9, 0x5C, 0xD4, 0x3C, 0xD0, 0x82, 0x0D,
            0x7E, 0xA6, 0x7B, 0x23, 0x27, 0x0F, 0x06, 0x64, 0x44, 0x45
        };

        private byte[] _DQ =
        {
            0x9E, 0x2C, 0x3B, 0x0B, 0xCA, 0x21, 0xBC, 0xD0, 0x85, 0xA8, 0x90, 0x78, 0xF8, 0x64, 0xE2, 0x7A, 0xAE, 0x3A,
            0xEF, 0xA3, 0x2F, 0x94, 0x79, 0x3C, 0xE9, 0x39, 0xD3, 0x9E, 0x41, 0x88, 0x03, 0x68, 0x94, 0x86, 0xD8, 0xCD,
            0x5A, 0x00, 0x40, 0x92, 0xE5, 0x58, 0x4A, 0x2C, 0x3D, 0x83, 0x9C, 0x42, 0x8F, 0x07, 0x9C, 0xA7, 0x79, 0x3A,
            0xE5, 0xC8, 0x2E, 0x87, 0x55, 0x94, 0xB0, 0xD0, 0xEE, 0x21
        };

        private byte[] _INVERSE =
        {
            0x8B, 0x43, 0x80, 0x33, 0x9E, 0x1A, 0x7C, 0x80, 0xB3, 0x37, 0x1D, 0x44, 0x0F, 0x05, 0x1C, 0x15, 0xF9, 0x7C,
            0x24, 0xD7, 0xC3, 0x51, 0xEB, 0x2F, 0x10, 0x7E, 0xAF, 0x64, 0x19, 0x34, 0x3E, 0xEE, 0xFD, 0x18, 0x8A, 0x10,
            0xF7, 0xA5, 0xFD, 0x37, 0x89, 0xE8, 0x96, 0x47, 0x10, 0x28, 0x35, 0xB3, 0xAC, 0x58, 0x6B, 0xC5, 0x09, 0x96,
            0xB8, 0xA0, 0x2E, 0x2E, 0x39, 0x78, 0x73, 0x36, 0xC2, 0x73
        };

        private byte[] _D =
        {
            0x50, 0xEF, 0xDC, 0xC2, 0x75, 0xBD, 0x67, 0xA0, 0x4E, 0x12, 0xED, 0x84, 0x8D, 0x8D, 0x03, 0x14, 0xA8, 0xB2,
            0x4A, 0xCB, 0x1A, 0x88, 0xC7, 0x51, 0x23, 0xE0, 0x20, 0xAF, 0x93, 0x4D, 0xE2, 0xB4, 0xF3, 0x14, 0xCF, 0xA7,
            0xDF, 0xB1, 0x13, 0xE5, 0x1A, 0x51, 0xCD, 0x8B, 0x48, 0x25, 0x15, 0x80, 0x71, 0x68, 0x1F, 0x22, 0x36, 0xD0,
            0xD2, 0xC4, 0x93, 0x16, 0xE0, 0x98, 0x8A, 0x6C, 0x68, 0xFC, 0xC0, 0x87, 0x84, 0x02, 0x4D, 0x59, 0x64, 0xF7,
            0x2B, 0x8D, 0xED, 0x82, 0xA1, 0xC1, 0x14, 0x8F, 0x01, 0x7C, 0x63, 0x68, 0xA9, 0x83, 0x2F, 0xDB, 0x4C, 0xC6,
            0x58, 0x70, 0x01, 0x9F, 0xCA, 0x53, 0x04, 0x59, 0x44, 0x00, 0x7E, 0x69, 0x66, 0xD5, 0x30, 0x1E, 0x8A, 0x1F,
            0x55, 0x8B, 0xEC, 0x54, 0x28, 0xA8, 0x1E, 0x99, 0xBC, 0x93, 0x9D, 0xF9, 0x0C, 0x8E, 0x48, 0x07, 0x0B, 0x04,
            0xDB, 0x11
        };

        private IniFileName m_ifConfig;

        private string m_szLoginAddr = "127.0.0.1";
        private int m_nLoginPort = 9958;
        private ushort m_usCurrentVersion;

        private string m_szGameSite = string.Empty;
        private string m_szRegisterSite = string.Empty;
        private string m_szRankingSite = string.Empty;
        private string m_szLogoutSite = string.Empty;
        private string m_szDownloadSite = string.Empty;
        private string m_szQueryAutoPatch = string.Empty;

        private string m_szAutoPatchAddr = string.Empty;
        private int m_nAutoPatchPort = 9528;

        private AsynchronousClientSocket m_pSocket;
        private PatchServer m_patchServer;

        public FrmMain()
        {
            InitializeComponent();

            // since we need transparent BG to images, we set it at startup
            AllowTransparency = true;
            TransparencyKey = Color.FromArgb(0xff00d8);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.FromArgb(0x00FFFFFF);

            byte[] file = File.ReadAllBytes("login.dat");
            SymmetricCryptoHandler crypto = new SymmetricCryptoHandler(
                new RSAParameters
                {
                    Modulus = _MODULUS,
                    Exponent = _EXPONENT,
                    P = _P,
                    DP = _DP,
                    Q = _Q,
                    DQ = _DQ,
                    D = _D,
                    InverseQ = _INVERSE
                }
            );

            byte[] decrypted = crypto.Decrypt(file);

            string[] fullAddr = Encoding.ASCII.GetString(decrypted, 0, decrypted.Length).TrimEnd('\0').Split(':');
            if (fullAddr.Length > 0)
                m_szLoginAddr = fullAddr[0];
            if (fullAddr.Length > 1)
                m_nLoginPort = int.Parse(fullAddr[1]);

            btnPlayHigh.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnPlayHigh.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnPlayLow.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnPlayLow.FlatAppearance.MouseOverBackColor = Color.Transparent;

            /**
             * Load culture info for multi-language application.
             */
            new LanguageManager().SetLanguage(CultureInfo.CurrentUICulture.Name);

            m_ifConfig = new IniFileName(Environment.CurrentDirectory + "\\AutoPatch.ini");
            LoadStringUrlTable();
        }

        public void LoadStringUrlTable()
        {
            string[] sites = m_ifConfig.GetEntryNames("Site");
            foreach (var entry in sites)
            {
                string value = m_ifConfig.GetEntryValue("Site", entry).ToString();
                switch (entry)
                {
                    case "Game":
                        m_szGameSite = value;
                        continue;
                    case "Register":
                        m_szRegisterSite = value;
                        continue;
                    case "Ranking":
                        m_szRankingSite = value;
                        continue;
                    case "Logout":
                        m_szLogoutSite = value;
                        continue;
                    case "Download":
                        m_szDownloadSite = value;
                        continue;
                    case "AutoPatchUrl":
                        m_szQueryAutoPatch = value;
                        continue;
                }
            }
        }

        #region Invoke to Update Interface

        public void Edit(Label control, LabelAsyncOperation op, object value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Label, LabelAsyncOperation, object>(Edit), control, op, value);
                return;
            }

            try
            {
                switch (op)
                {
                    case LabelAsyncOperation.Text:
                        control.Text = value.ToString();
                        break;
                    case LabelAsyncOperation.TextColor:
                        control.ForeColor = (Color) value;
                        break;
                }
            }
            catch
            {
                // do nothing, just dont update
            }
        }

        public void Edit(ProgressBar control, ProgressBarAsyncOperation op, object value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ProgressBar, ProgressBarAsyncOperation, object>(Edit), control, op, value);
                return;
            }

            try
            {
                switch (op)
                {
                    case ProgressBarAsyncOperation.Value:
                        control.Value = (int) value;
                        break;
                    case ProgressBarAsyncOperation.Min:
                        control.Minimum = (int) value;
                        break;
                    case ProgressBarAsyncOperation.Max:
                        control.Minimum = (int) value;
                        break;
                }
            }
            catch
            {
                // do nothing, just dont update
            }
        }

        #endregion

        #region Version

        private void LoadVersion()
        {
            using (var reader = new StreamReader("version.dat"))
            {
                string szVersion = reader.ReadLine();
                if (szVersion == null || string.IsNullOrEmpty(szVersion))
                    Environment.Exit(3);
                Kernel.ActualVersion = m_usCurrentVersion = ushort.Parse(szVersion);
                Edit(lblGameVersion, LabelAsyncOperation.Text,
                    LanguageManager.GetString("StrGameVersion", m_usCurrentVersion));
            }

            Edit(lblUpdaterVersion, LabelAsyncOperation.Text,
                LanguageManager.GetString("StrAutoUpdateVersion", Kernel.ActualVersion, Kernel.Version));
        }

        #endregion

        #region Form Events

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LoadVersion();
            ConnectToAutoUpdateServer();
        }

        #region Mouse Hover Buttons

        private void btnRegister_MouseEnter(object sender, EventArgs e)
        {
            btnRegister.Image = Resources.Register02;
        }

        private void btnRegister_MouseLeave(object sender, EventArgs e)
        {
            btnRegister.Image = Resources.Register01;
        }

        private void btnDownload_MouseEnter(object sender, EventArgs e)
        {
            btnDownload.Image = Resources.Download02;
        }

        private void btnDownload_MouseLeave(object sender, EventArgs e)
        {
            btnDownload.Image = Resources.Download;
        }

        private void btnRanking_MouseEnter(object sender, EventArgs e)
        {
            btnRanking.Image = Resources.Ranking02;
        }

        private void btnRanking_MouseLeave(object sender, EventArgs e)
        {
            btnRanking.Image = Resources.Ranking;
        }

        private void btnSite_MouseEnter(object sender, EventArgs e)
        {
            btnSite.Image = Resources.Office02;
        }

        private void btnSite_MouseLeave(object sender, EventArgs e)
        {
            btnSite.Image = Resources.Office01;
        }

        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.Image = Resources.Exit02;
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.Image = Resources.Exit01;
        }

        private void btnPlayHigh_MouseEnter(object sender, EventArgs e)
        {
            btnPlayHigh.Image = Resources.high021;
        }

        private void btnPlayHigh_MouseLeave(object sender, EventArgs e)
        {
            btnPlayHigh.Image = Resources.high031;
        }

        private void btnPlayLow_MouseEnter(object sender, EventArgs e)
        {
            btnPlayLow.Image = Resources.low021;
        }

        private void btnPlayLow_MouseLeave(object sender, EventArgs e)
        {
            btnPlayLow.Image = Resources.low031;
        }

        #endregion

        #endregion

        #region Buttons Click

        private void btnPlayHigh_Click(object sender, EventArgs e)
        {
        }

        private void btnPlayLow_Click(object sender, EventArgs e)
        {
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            LaunchSite(m_szRegisterSite);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            LaunchSite(m_szDownloadSite);
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            LaunchSite(m_szRankingSite);
        }

        private void btnSite_Click(object sender, EventArgs e)
        {
            LaunchSite(m_szGameSite);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            LaunchSite(m_szLogoutSite);

            Close();
        }

        #endregion

        #region Anti Cheat Routines

        public string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up &&
                            n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .OrderByDescending(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .Select(n => n.GetPhysicalAddress())
                .FirstOrDefault()?.ToString();
        }

        #endregion

        #region Open Browser

        public void LaunchSite(string url)
        {
            Process.Start(url);
        }

        #endregion

        #region Game Startup



        #endregion

        #region Remote File Check

        public bool RemoteFileExists(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException)
            {
                /* A WebException will be thrown if the status of the response is not `200 OK` */
                return false;
            }
            finally
            {
                // Don't forget to close your response.
                response?.Close();
            }
        }

        public string ReadStringFromUrl(string url)
        {
            if (!RemoteFileExists(url))
                return null;

            try
            {
                using (var web = new WebClient())
                    return web.DownloadString(url);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Auto Patch Connection

        public void ConnectToAutoUpdateServer()
        {
            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrSearchForOpenInstances"));
            var procList = Process.GetProcessesByName("Conquer");
            if (procList.Length > 0)
            {
                if (MessageBox.Show(this, LanguageManager.GetString("StrConquerRunning"), LanguageManager.GetString("StrConquerRunningTitle"), MessageBoxButtons.YesNo) 
                    == DialogResult.Yes)
                {
                    foreach (var proc in procList)
                        proc.Kill();
                }
                else
                {
                    NoDownload(false);
                    return;
                }
            }

            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString(""));
            string autoPatch = ReadStringFromUrl(m_szQueryAutoPatch);
            bool connected = false;
            if (string.IsNullOrEmpty(autoPatch))
            {
                connected = AttemptConnectionFromFile();
            }
            else
            {
                string[] split = autoPatch.Replace("\n", "").Split(':');
                connected = Connect(split[0], split[1]);
            }

            if (m_patchServer == null || !connected)
            {
                if (MessageBox.Show(this, LanguageManager.GetString("StrCouldNotConnectToAutoPatch")) == DialogResult.Yes)
                    LaunchSite(m_szDownloadSite);

                Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrCouldNotFindUpdaterServer"));
                NoDownload();
                return;
            }
            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrUpdaterServerFound"));
            NoDownload();
        }

        private bool AttemptConnectionFromFile()
        {
            bool connected = false;
            string[] szServers = m_ifConfig.GetEntryNames("Server");
            foreach (var ipServer in szServers)
            {
                string[] split = m_ifConfig.GetEntryValue("Server", ipServer).ToString().Split(' ');
                try
                {
                    connected = Connect(split[0], split[1]);
                    break;
                }
                catch
                {
                    continue;
                }
            }

            return connected;
        }

        private bool Connect(string ip, string port)
        {
            try
            {
                m_pSocket = new AsynchronousClientSocket("AutoPatch", AddressFamily.InterNetwork, SocketType.Stream,
                    ProtocolType.Tcp);
                m_pSocket.ConnectTo(ip, int.Parse(port));
                m_patchServer = new PatchServer(m_pSocket, m_pSocket);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Update Routines

        public void NoDownload(bool allowStart = true)
        {
            if (!allowStart)
                return;

            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrClientUpdatedOK"));
            btnExit.Enabled = true;
            btnPlayHigh.Enabled = true;
            btnPlayLow.Enabled = true;
        }

        #endregion
    }

    public enum LabelAsyncOperation
    {
        None,
        Text,
        TextColor
    }

    public enum ProgressBarAsyncOperation
    {
        None,
        Value,
        Min,
        Max
    }
}