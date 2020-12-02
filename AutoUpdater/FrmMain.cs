#region Header and Copyright

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

#define NO_INJECTION // if you want to use the pure launcher, uncomment this

#region References

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using AutoUpdater.Properties;
using AutoUpdater.Sockets;
using AutoUpdater.Sockets.Updater;
using AutoUpdaterCore;
using AutoUpdaterCore.Sockets;
using AutoUpdaterCore.Sockets.Packets;
using Timer = System.Windows.Forms.Timer;

#endregion

namespace AutoUpdater
{
    public partial class FrmMain : Form
    {
        private const double _KBYTE = 1024;
        private const double _MBYTE = _KBYTE * 1024;
        private const double _GBYTE = _MBYTE * 1024;

        private readonly IniFileName m_ifConfig;
        private int m_nAutoPatchPort = 9528;
        private PatchServer m_patchServer;

        private AsynchronousClientSocket m_pSocket;

        private string m_szAutoPatchAddr = string.Empty;
        private string m_szDownloadSite = string.Empty;

        private string m_szGameSite = string.Empty;

        private string m_szLogoutSite = string.Empty;
        private string m_szQueryAutoPatch = string.Empty;
        private string m_szRankingSite = string.Empty;
        private string m_szRegisterSite = string.Empty;
        private string m_szPrivacyTerms = "https://worldconquer.online/Privacy";
        private string m_szTermsOfService = "https://worldconquer.online/TermsOfUse";

        private string m_szOpenAfterClose = "";

        private bool m_bIsClosing = false;
        private bool m_bInternalCloseRequest = false;
        private UpdateDownloadType m_actuallyDownloading = UpdateDownloadType.None;

        private ushort m_usCurrentVersion;
        private ushort m_usClientVersion;

        private Queue<string> m_queueNextDownloads = new Queue<string>();
        private Queue<string> m_queueDoneDownloads = new Queue<string>();

        private int m_nCurrentlyBytesDownloaded = 0;
        private int m_nCurrentDownloading = 0;
        private int m_nTotalDownloads = 0;
        private long m_nTotalDownloadSize = 0;

        private WebClient m_wcClient;
        private Stopwatch m_swStopwatch = new Stopwatch();
        private int m_nLastDownloadTick = 0;

        List<Process> m_lOpenClients = new List<Process>();

        public FrmMain()
        {
            InitializeComponent();

            // since we need transparent BG to images, we set it at startup
            AllowTransparency = true;
            TransparencyKey = Color.FromArgb(0xff00d8);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.FromArgb(0x00FFFFFF);
            
            btnPlayHigh.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnPlayHigh.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnPlayLow.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnPlayLow.FlatAppearance.MouseOverBackColor = Color.Transparent;

            /*
             * Load culture info for multi-language application.
             */
            new LanguageManager().SetLanguage(CultureInfo.CurrentUICulture.Name);

            m_ifConfig = new IniFileName(Environment.CurrentDirectory + "\\AutoPatch.ini");
            LoadStringUrlTable();
            // we will download everything again if needed. But we wont be keeping the folder with content after a succesfull (or failed) update.
            DeleteTempFolder();
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

            lnkPrivacy.Text = LanguageManager.GetString("StrTopLabel");
            lnkTos.Text = LanguageManager.GetString("StrTosLabel");
        }

        #region Version

        private void LoadVersion()
        {
            m_usClientVersion = Convert.ToUInt16(m_ifConfig.GetEntryValue("Client", "Version").ToString());
            using (var reader = new StreamReader("version.dat"))
            {
                string szVersion = reader.ReadLine();
                if (szVersion == null || string.IsNullOrEmpty(szVersion))
                    Environment.Exit(3);
                Kernel.ActualVersion = m_usCurrentVersion = ushort.Parse(szVersion);
                Edit(lblGameVersion, LabelAsyncOperation.Text, LanguageManager.GetString("StrGameVersion", m_usCurrentVersion));
            }
            Edit(lblUpdaterVersion, LabelAsyncOperation.Text, LanguageManager.GetString("StrAutoUpdateVersion", m_usClientVersion, Kernel.Version));
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

        #region Update Routines

        public void NoDownload(UpdateReturnMessage msg, bool allowStart = true)
        {
            HideDownloadBar();

            switch (msg)
            {
                case UpdateReturnMessage.Success:
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrClientUpdatedOK"));
                    break;
                case UpdateReturnMessage.UnknownFail:
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrClientUpdatedError"));
                    break;
                case UpdateReturnMessage.ConnectionError:
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrConnectionError"));
                    break;
                case UpdateReturnMessage.OpenClientError:
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrOpenClientError"));
                    break;
                case UpdateReturnMessage.LoginNotAllowed:
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrLoginNotAllowed"));
                    break;
                case UpdateReturnMessage.DoubleClient:
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrDoubleClient"));
                    break;
            }

            Edit(btnExit, ButtonAsyncOperation.Enable, true);

            if (!allowStart)
                return;

            Edit(btnPlayLow, ButtonAsyncOperation.Enable, true);
            Edit(btnPlayLow, ButtonAsyncOperation.Visible, true);
            Edit(btnPlayHigh, ButtonAsyncOperation.Enable, true);
            Edit(btnPlayHigh, ButtonAsyncOperation.Visible, true);
        }

        public void PrepareToDownload(UpdateDownloadType type, List<string> strs, PatchServer server)
        {
            HideDownloadBar();
            if (strs.Count < 2)
            {
                if (type == UpdateDownloadType.UpdaterPatch)
                {
                    MsgRequestInfo mri = new MsgRequestInfo();
                    mri.CurrentVersion = m_usCurrentVersion;
                    server.Send(mri);

                    Kernel.Stage = AutoPatchStage.WaitingForGamePatchs;
                    Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrLookingForGameUpdates"));
                }
                else
                {
                    NoDownload(UpdateReturnMessage.Success);
                }
                return;
            }

            m_actuallyDownloading = type;
            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrCalculatingDownloadSize"));

            string domain = strs[0];
            if (!domain.EndsWith("/"))
                domain += "/";

            m_nTotalDownloadSize = 0;
            m_nCurrentDownloading = 0;
            m_nTotalDownloads = 0;

            Edit(lblDownloadStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrLabelCalculatingDownloadAmount", m_nTotalDownloads, ParseFileSize(m_nTotalDownloadSize)));
            Edit(lblDownloadStatus, LabelAsyncOperation.Visible, true);

            for (int i = 1; i < strs.Count; i++)
            {
                if (!strs[i].EndsWith(".exe"))
                    strs[i] += ".exe";

                if (!RemoteFileExists($"{domain}{strs[i]}"))
                    continue;

                m_nTotalDownloadSize += FetchFileSize($"{domain}{strs[i]}");
                m_nTotalDownloads++;
                m_queueNextDownloads.Enqueue($"{domain}{strs[i]}");
                Edit(lblDownloadStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrLabelCalculatingDownloadAmount", m_nTotalDownloads, ParseFileSize(m_nTotalDownloadSize)));
            }

            Edit(lblDownloadStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrLabelCalculatingDownloadAmount", m_nTotalDownloads, ParseFileSize(m_nTotalDownloadSize)));
            Edit(pbDownload, ProgressBarAsyncOperation.Value, 0);
            Edit(pbDownload, ProgressBarAsyncOperation.Max, m_nTotalDownloadSize);
            ShowDownloadBar();
            StartDownloading();
        }

        public void ShowDownloadBar()
        {
            Edit(lblDownloadStatus, LabelAsyncOperation.Visible, true);
            Edit(panelStatus, PanelAsyncOperation.Visible, false);
            Edit(panelProgressbar, PanelAsyncOperation.Visible, true);
        }

        public void HideDownloadBar()
        {
            Edit(lblDownloadStatus, LabelAsyncOperation.Visible, false);
            Edit(panelStatus, PanelAsyncOperation.Visible, true);
            Edit(panelProgressbar, PanelAsyncOperation.Visible, false);
        }

        private string GetTempDownloadPath(string name)
        {
            string path = Environment.CurrentDirectory + "\\AutoPatch";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            new DirectoryInfo(path).Attributes &= ~FileAttributes.ReadOnly;
            path += "\\temp";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            new DirectoryInfo(path).Attributes &= ~FileAttributes.ReadOnly;
            return $"{path}\\{name}";
        }

        #endregion

        #region Downloading

        private void StartDownloading()
        {
            if (m_queueNextDownloads.Count == 0 || !string.IsNullOrEmpty(m_szOpenAfterClose))
            {
                if (m_queueDoneDownloads.Count > 0)
                    BeginInstall();
                else RequestPatches(AutoUpdateRequestType.CheckForGameUpdates, m_patchServer);
                return;
            }

            string download = m_queueNextDownloads.Dequeue();
            string[] parsed = download.Split('/');
            string fileName = parsed[parsed.Length - 1];
            m_nCurrentDownloading++;
            m_queueDoneDownloads.Enqueue(download);

            Edit(lblDownloadStatus, LabelAsyncOperation.Text,
                LanguageManager.GetString("StrLabelDownloading", m_nCurrentDownloading, m_nTotalDownloads,
                    ParseFileSize(pbDownload.Value), ParseFileSize(m_nTotalDownloadSize), ParseDownloadSpeed(0)));

            m_swStopwatch = new Stopwatch();
            m_swStopwatch.Start();
            m_wcClient = new WebClient();
            m_wcClient.DownloadProgressChanged += DownloadFileProgressChanged;
            m_wcClient.DownloadFileCompleted += ClientOnDownloadFileCompleted;
            m_wcClient.DownloadFileAsync(new Uri(download), GetTempDownloadPath(fileName));
        }

        private void ClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            m_nCurrentlyBytesDownloaded = 0;
            m_wcClient.Dispose();
            m_swStopwatch.Stop();
            m_nLastDownloadTick = Environment.TickCount;
            StartDownloading();
        }

        private void DownloadFileProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Edit(pbDownload, ProgressBarAsyncOperation.Value, pbDownload.Value + (e.BytesReceived - m_nCurrentlyBytesDownloaded));
            m_nCurrentlyBytesDownloaded = (int)e.BytesReceived;

            if (Environment.TickCount - m_nLastDownloadTick > 500)
            {
                Edit(lblDownloadStatus, LabelAsyncOperation.Text,
                    LanguageManager.GetString("StrLabelDownloading", m_nCurrentDownloading, m_nTotalDownloads,
                        ParseFileSize(pbDownload.Value), ParseFileSize(m_nTotalDownloadSize),
                        ParseDownloadSpeed((long) (e.BytesReceived / m_swStopwatch.Elapsed.TotalSeconds))));
                m_nLastDownloadTick = Environment.TickCount;
            }
        }

        private void BeginInstall()
        {
            HideDownloadBar();
            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrStartInstallUpdates"));
            string fullPath;
            while (m_queueDoneDownloads.Count > 0 && !string.IsNullOrEmpty(fullPath = m_queueDoneDownloads.Dequeue()))
            {
                string[] parsed = fullPath.Split('/');
                string fileName = parsed[parsed.Length - 1];
                string tempName = fileName.Replace(".exe", "");
                if (int.TryParse(tempName, out int idPatch) && idPatch > 10000)
                {
                    m_szOpenAfterClose = GetTempDownloadPath(fileName);
                    m_bInternalCloseRequest = true;
                    Close();
                    return;
                }

                Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrInstallingUpdates", fileName));

                if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                    fileName += ".exe";

                string localPath = GetTempDownloadPath(fileName);
                Process process = Process.Start(localPath, $"-s -d \"{Environment.CurrentDirectory}\"");
                process?.WaitForExit();
                process?.Close();
                process?.Dispose();
                LoadVersion();
            }

            DeleteTempFolder();

            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrPatchsInstalled"));

            Kernel.Stage = AutoPatchStage.WaitingForUpdaterPatchs;
            RequestPatches(AutoUpdateRequestType.CheckForLauncherUpdates, m_patchServer);
        }

        private void DeleteTempFolder()
        {
            string temp = GetTempDownloadPath("");
            Directory.Delete(temp.Substring(0, temp.Length - 1), true);
        }

        #endregion

        #region Links

        private void lnkPrivacy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FrmWebBrowser(m_szPrivacyTerms).ShowDialog(this);
        }

        private void lnkTos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FrmWebBrowser(m_szTermsOfService).ShowDialog(this);
        }

        #endregion

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
                    case LabelAsyncOperation.Visible:
                        control.Visible = (bool) value;
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
                        control.Value = (int)((long)value);
                        break;
                    case ProgressBarAsyncOperation.Min:
                        control.Minimum = (int)((long)value);
                        break;
                    case ProgressBarAsyncOperation.Max:
                        control.Maximum = (int)((long)value);
                        break;
                }
            }
            catch
            {
                // do nothing, just dont update
            }
        }

        public void Edit(Panel control, PanelAsyncOperation op, object value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Panel, PanelAsyncOperation, object>(Edit), control, op, value);
                return;
            }

            try
            {
                switch (op)
                {
                    case PanelAsyncOperation.Visible:
                        control.Visible = (bool)value;
                        break;
                }
            }
            catch
            {
                // do nothing, just dont update
            }
        }

        public void Edit(Button control, ButtonAsyncOperation op, object value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Button, ButtonAsyncOperation, object>(Edit), control, op, value);
                return;
            }

            try
            {
                switch (op)
                {
                    case ButtonAsyncOperation.Visible:
                        control.Visible = (bool)value;
                        break;
                    case ButtonAsyncOperation.Enable:
                        control.Enabled = (bool) value;
                        break;
                }
            }
            catch
            {
                // do nothing, just dont update
            }
        }

        #endregion

        #region Form Events

        private void FrmMain_Load(object sender, EventArgs e)
        {
            DeleteTempFolder();
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
            SetGameMode(GameMode.HighDefinition);
            Play();
        }

        private void btnPlayLow_Click(object sender, EventArgs e)
        {
            SetGameMode(GameMode.LowDefinition);
            Play();
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
            Close();
        }

        #endregion
        
        #region Remote File Check

        public bool RemoteFileExists(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "HEAD";
            request.UserAgent = "World-Conquer-Online-Auto-Patcher";
            try
            {
                response = (HttpWebResponse) request.GetResponse();
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

        public long FetchFileSize(string url)
        {
            if (!RemoteFileExists(url))
                return 0;

            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";
            req.Timeout = 1000;
            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
                if (long.TryParse(resp.Headers.Get("Content-Length"), out var contentLength))
                {
                    return contentLength;
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                resp?.Close();
            }
            return 0;
        }

        public string ParseFileSize(long size)
        {
            if (size > _GBYTE)
                return $"{size / _GBYTE:N2} GB";
            if (size > _MBYTE)
                return $"{size / _MBYTE:N2} MB";
            if (size > _KBYTE)
                return $"{size/_KBYTE:N2} KB";
            return $"{size} B";
        }

        public string ParseDownloadSpeed(long amount)
        {
            if (amount > _GBYTE)
                return $"{amount / _GBYTE:N2} GB/s";
            if (amount > _MBYTE)
                return $"{amount / _MBYTE:N2} MB/s";
            if (amount > _KBYTE)
                return $"{amount / _KBYTE:N2} KB/s";
            return $"{amount} B/s";
        }

        #endregion

        #region Auto Patch Connection

        public void ConnectToAutoUpdateServer()
        {
            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrSearchForOpenInstances"));
            var procList = Process.GetProcessesByName("Conquer");
            if (procList.Length > 0)
            {
                if (MessageBox.Show(this, LanguageManager.GetString("StrConquerRunning"),
                        LanguageManager.GetString("StrConquerRunningTitle"), MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    foreach (var proc in procList)
                    {
                        try
                        {
                            proc.Kill();
                        }
                        catch
                        {
                            // process might have been exited already
                        }
                    }
                }
                else
                {
                    NoDownload(UpdateReturnMessage.OpenClientError, false);
                    return;
                }
            }

            Edit(lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrConnectingToServer"));

#if !DEBUG
            string autoPatch = ReadStringFromUrl(m_szQueryAutoPatch);
#else
            //string autoPatch = "127.0.0.1:9528";
            string autoPatch = ReadStringFromUrl(m_szQueryAutoPatch);
#endif


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
                if (MessageBox.Show(this, LanguageManager.GetString("StrCouldNotConnectToAutoPatch"),
                        LanguageManager.GetString("StrCouldNotConnectToAutoPatchTitle"),
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    LaunchSite(m_szDownloadSite);

                Edit(lblCenterStatus, LabelAsyncOperation.Text,
                    LanguageManager.GetString("StrCouldNotFindUpdaterServer"));
                NoDownload(UpdateReturnMessage.ConnectionError);
                return;
            }
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
                }
            }

            return connected;
        }

        private bool Connect(string ip, string port)
        {
            try
            {
                m_pSocket = new ClientSocket();
                m_pSocket.ConnectTo(ip, int.Parse(port));
                m_patchServer = new PatchServer(m_pSocket, m_pSocket);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RequestPatches(AutoUpdateRequestType mode, PatchServer server)
        {
            if (!Kernel.HasAgreedPrivacy)
            {
                NoDownload(UpdateReturnMessage.PrivacyNotAccepted, false);
                return;
            }

            MsgRequestInfo msg = new MsgRequestInfo
            {
                Mode = mode
            };
            switch (mode)
            {
                case AutoUpdateRequestType.CheckForLauncherUpdates:
                    msg.CurrentVersion = m_usClientVersion;
                    break;
                case AutoUpdateRequestType.CheckForGameUpdates:
                    msg.CurrentVersion = m_usCurrentVersion;
                    break;
            }
            server.Send(msg);
        }

        #endregion

        #region Internal Packet processing

        public void ProcessPacket(PatchServer server, byte[] buffer)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<PatchServer, byte[]>(ProcessPacket), server, buffer);
                return;
            }

            if (buffer.Length < 4)
                return;

            PacketType type = (PacketType) BitConverter.ToUInt32(buffer, 2);
            switch (type)
            {
                case PacketType.MsgClientInfo:
                    MsgClientInfo msg = new MsgClientInfo(buffer);
                    List<string> list = msg.GetStrings();
                    if (list.Count <= 0)
                    {
                        Kernel.HasAgreedPrivacy = true;
                        RequestPatches(AutoUpdateRequestType.CheckForLauncherUpdates, server);
                        return;
                    }

                    string strPrivacyDate = m_ifConfig.GetEntryValue("Config", "TermsOfPrivacy").ToString();
                    if (!DateTime.TryParse(strPrivacyDate, out DateTime date) 
                        || !DateTime.TryParse(list[0], out DateTime serverTime) 
                        || serverTime > date)
                    {
                        if (new FrmTermsOfPrivacy(m_szPrivacyTerms).ShowDialog(this) != DialogResult.OK)
                        {
                            NoDownload(UpdateReturnMessage.PrivacyNotAccepted, false);
                            return;
                        }

                        m_ifConfig.SetValue("Config", "TermsOfPrivacy", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }

                    Kernel.HasAgreedPrivacy = true;
                    Kernel.Stage = AutoPatchStage.WaitingForUpdaterPatchs;
                    RequestPatches(AutoUpdateRequestType.CheckForLauncherUpdates, server);
                    break;
            }
        }

        #endregion

        #region Form Close events

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bInternalCloseRequest)
                return;

            if (MessageBox.Show(this, LanguageManager.GetString("StrCloseWindowMsg"),
                    LanguageManager.GetString("StrCloseWindowTitle"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) ==
                DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            m_bIsClosing = true;
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var game in m_lOpenClients)
            {
                try
                {
                    game.Kill();
                    game.Close();
                    game.Dispose();
                }
                catch
                {

                }
            }

            if (!string.IsNullOrEmpty(m_szOpenAfterClose))
            {
                Process.Start(m_szOpenAfterClose);
            }
            else
            {
                LaunchSite(m_szLogoutSite);
            }
        }

        #endregion

        #region Start Game

        private void Play()
        {
            const string fileName = "Conquer.exe";
            string[] filesToCheck =
            {
                fileName,
#if !NO_INJECTION
                "AutoPatchLoader.exe",
                "Loader.dll",
                "config.ini",
#endif
            };

#if !NO_INJECTION
            string path = $"{Environment.CurrentDirectory}\\AutoPatchLoader.exe";
#else
            string path = $"{Environment.CurrentDirectory}\\{fileName}";
#endif
            foreach (var file in filesToCheck)
            {
                string verifyPath = $"{Environment.CurrentDirectory}\\{file}";
                if (!File.Exists(verifyPath))
                {
                    MessageBox.Show(this, LanguageManager.GetString("StrFileMissing", file),
                        LanguageManager.GetString("StrFileMissingTitle"),
                        MessageBoxButtons.OK);
                    return;
                }
            }

            Process game = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = path,
#if !NO_INJECTION
                    Arguments = "whitenull",
#else
                    Arguments = "blacknull",
#endif
                }
            };

            game.Start();
#if !NO_INJECTION
            game.WaitForExit();
#else
            m_lOpenClients.Add(game);
#endif

#if !NO_INJECTION
            try
            {
                Process addGame = Process.GetProcessById(game.ExitCode);
                m_lOpenClients.Add(addGame);
            }
            catch
            {

            }
#endif

            //if (!m_pAntiCheatTimer.Enabled)
            //{
            //OnTimer(null, null);
            //m_pAntiCheatTimer.Start();
            //}
        }

        private void SetGameMode(GameMode mode)
        {
            IniFileName ini = new IniFileName(Environment.CurrentDirectory + @"\ini\GameSetUp.ini");

            int lowTimes = 0;
            int highTimes = 0;
            try
            {
                lowTimes = int.Parse(ini.GetEntryValue("GameMode", "LowTimes").ToString());
                highTimes = int.Parse(ini.GetEntryValue("GameMode", "HighTimes").ToString());
            }
            catch
            {

            }

            switch (mode)
            {
                case GameMode.HighDefinition:
                    highTimes++;
                    break;
                case GameMode.LowDefinition:
                    lowTimes++;
                    break;
            }

            ini.SetValue("GameMode", "LowTimes", lowTimes);
            ini.SetValue("GameMode", "GameModeRecord", (int)mode);
            ini.SetValue("GameMode", "HighTimes", highTimes);
        }

#endregion
    }

    public enum GameMode
    {
        LowDefinition,
        HighDefinition
    }

    public enum LabelAsyncOperation
    {
        None,
        Text,
        TextColor,
        Visible
    }

    public enum ProgressBarAsyncOperation
    {
        None,
        Value,
        Min,
        Max
    }

    public enum PanelAsyncOperation
    {
        None,
        Visible
    }

    public enum ButtonAsyncOperation
    {
        None,
        Visible,
        Enable
    }

    public enum UpdateReturnMessage
    {
        Success,
        ConnectionError,
        OpenClientError,
        UnknownFail,
        PrivacyNotAccepted,
        LoginNotAllowed,
        DoubleClient
    }
}