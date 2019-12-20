#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchServer - Program.cs
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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using AutoPatchServer.Sockets.Updater;
using AutoUpdaterCore;
using AutoUpdaterCore.Interfaces;
using AutoUpdaterCore.Windows;

namespace AutoPatchServer
{
    class Program
    {
        public static LogWriter Log = new LogWriter(Environment.CurrentDirectory);

        public static InputConsoleBox StatisticBox = new InputConsoleBox(0, 9);

        private static readonly InputConsoleBox m_outputBox = new InputConsoleBox(9, 16)
        {
            AutoDraw = true
        };

        private static readonly InputConsoleBox m_inputBox = new InputConsoleBox(26, 1)
        {
            InputPrompt = "Command: "
        };

        static void Main(string[] args)
        {
            Thread uiThread = new Thread(UiThread) { Name = "UpdateServerUiThread", Priority = ThreadPriority.BelowNormal };
            uiThread.Start();

            WriteLog($"Initializing update server...");
            WriteLog($"Reading config file...");
            ReadConfigFile();

            UpdateSocket socket = new UpdateSocket();
            socket.Bind("0.0.0.0", Kernel.ListenPort);
            socket.Listen(10);

            HandleCommands();
            uiThread.Abort(0);
        }

        private static void HandleCommands()
        {
            string command;
            while ((command = m_inputBox.ReadLine()) != "exit")
            {
                if (string.IsNullOrEmpty(command))
                    continue;

                WriteLog(command, LogType.CONSOLE);

                string[] parsed = command.Split(' ');
                switch (parsed[0].ToLower())
                {
                    case "add":
                        if (parsed.Length < 2)
                            break;

                        if (parsed.Length == 2)
                        {
                            if (int.TryParse(parsed[1], out int version))
                            {
                                UpdatesManager.AddPatch(new PatchStructure
                                {
                                    To = version
                                }, false);
                            }
                        }
                        else if (parsed.Length == 4)
                        {
                            int from = 0, to = 0;
                            string fileName = parsed[3];

                            if (!int.TryParse(parsed[1], out from) || !int.TryParse(parsed[2], out to))
                                break;

                            UpdatesManager.AddPatch(new PatchStructure
                            {
                                From = from, To = to, FileName = fileName
                            }, false);
                        }
                        break;

                    case "remove":
                        if (parsed.Length < 2)
                            break;

                        if (int.TryParse(parsed[1], out int remVersion))
                        {
                            UpdatesManager.RemovePatch(remVersion, false);
                        }
                        break;

                    case "test":
                        if (parsed.Length < 2)
                            break;

                        if (parsed[1] == "download_list")
                        {
                            if (parsed.Length < 3 || !int.TryParse(parsed[2], out int idPatch))
                                break;

                            foreach (var patch in UpdatesManager.GetDownloadList(idPatch))
                            {
                                if (patch.From > 0)
                                {
                                    WriteLog($"Patch[From:{patch.From}][To:{patch.To}][File:{patch.FileName}]");
                                }
                                else
                                {
                                    WriteLog($"Patch[To:{patch.To}][File:{patch.FileName}]");
                                }
                            }
                        }

                        break;
                }
            }
        }

        private static void ReadConfigFile()
        {
            const string configFile = "AutoUpdater.xml";
            string path = Environment.CurrentDirectory + "\\" + configFile;
            if (!File.Exists(path))
            {
                MyXml create = new MyXml(path);
                create.AddNewNode("https://ftwmasters.com.br/patches", null, "DownloadUrl", "Config");
                create.AddNewNode("9528", null, "ListenPort", "Config");
                create.AddNewNode("10000", null, "LatestUpdaterVersion", "Config");
                create.AddNewNode("4000", null, "LatestGameVersion", "Config");
                create.AddNewNode("2019-10-22 00:00:00", null, "PrivacyTermsUpdate", "Config");
                create.AddNewNode("", null, "AllowedPatches", "Config");
                //create.AddNewNode("0", null, "Count", "Config", "AllowedPatches");
                create.AddNewNode("", null, "BundlePatches", "Config");
                //create.AddNewNode("0", null, "Count", "Config", "BundlePatches");
            }

            Kernel.MyXml = new MyXml(path);
            if (int.TryParse(Kernel.MyXml.GetValue("Config", "ListenPort"), out int port))
                Kernel.ListenPort = port;
            WriteLog($"Server will listen to port: {Kernel.ListenPort}", LogType.CONSOLE);
            WriteLog("If you want to change the server port, write `exit` and edit AutoUpdater.xml file.", LogType.CONSOLE);

            if (int.TryParse(Kernel.MyXml.GetValue("Config", "LatestUpdaterVersion"), out int updater))
                Kernel.LatestUpdaterPatch = updater;
            WriteLog($"Latest updater client version: {Kernel.LatestUpdaterPatch}", LogType.CONSOLE);

            if (int.TryParse(Kernel.MyXml.GetValue("Config", "LatestGameVersion"), out int game))
                Kernel.LatestGamePatch = game;
            WriteLog($"Latest game client version: {Kernel.LatestGamePatch}", LogType.CONSOLE);

            if (!string.IsNullOrEmpty(Kernel.MyXml.GetValue("Config", "DownloadUrl")))
                Kernel.DownloadUrl = Kernel.MyXml.GetValue("Config", "DownloadUrl");
            WriteLog($"Client will download files from: {Kernel.DownloadUrl}", LogType.CONSOLE);
            if (!string.IsNullOrEmpty(Kernel.MyXml.GetValue("Config", "PrivacyTermsUpdate")))
                Kernel.PrivacyTermsUpdate = DateTime.Parse(Kernel.MyXml.GetValue("Config", "PrivacyTermsUpdate"));
            WriteLog($"Privacy Terms last updated: {Kernel.PrivacyTermsUpdate}", LogType.CONSOLE);

            foreach (XmlNode node in Kernel.MyXml.GetAllNodes("Config", "AllowedPatches"))
            {
                UpdatesManager.AddPatch(new PatchStructure
                {
                    To = int.Parse(Kernel.MyXml.GetValue("Config", "AllowedPatches", $"Patch[@id='{node.Attributes["id"].Value}']")),
                    FileName = Kernel.MyXml.GetValue("Config", "AllowedPatches", $"Patch[@id='{node.Attributes["id"].Value}']")
                }, true);
            }

            foreach (XmlNode node in Kernel.MyXml.GetAllNodes("Config", "BundlePatches"))
            {
                UpdatesManager.AddPatch(new PatchStructure
                {
                    From = int.Parse(Kernel.MyXml.GetValue("Config", "BundlePatches", $"Patch[@id='{node.Attributes["id"].Value}']", "From")),
                    To = int.Parse(Kernel.MyXml.GetValue("Config", "BundlePatches", $"Patch[@id='{node.Attributes["id"].Value}']", "To")),
                    FileName = Kernel.MyXml.GetValue("Config", "BundlePatches", $"Patch[@id='{node.Attributes["id"].Value}']", "FileName")
                }, true);
            }
        }
        
        private static void UiThread()
        {
            string[] baseLine =
            {
                $"||{{0,-{(Console.BufferWidth-6)/2}}}||{{1,-{(Console.BufferWidth-6)/2}}}||",
                $"||{{0,-{(Console.BufferWidth-4)}}}||"
            };
            while (true)
            {
                Console.Title = $"FTW! Auto Update Server - {DateTime.Now:yyyy/MM/dd dddd HH:mm:ss}";

                StatisticBox.Write("".PadRight(Console.BufferWidth, '=')); // first line 0
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write(string.Format(baseLine[1], ""));
                StatisticBox.Write("".PadRight(Console.BufferWidth, '=')); // last line 8
                StatisticBox.Draw();
                Thread.Sleep(1000);
            }
        }

        public static void WriteLog(string log, LogType type = LogType.MESSAGE)
        {
            string msg = Log.SaveLog(log, type);
            InputConsoleBox.Colors color = InputConsoleBox.Colors.LightWhite;

            switch (type)
            {
                case LogType.DEBUG:
                    color = InputConsoleBox.Colors.LightBlue;
                    break;
                case LogType.ERROR:
                    color = InputConsoleBox.Colors.LightRed;
                    break;
                case LogType.EXCEPTION:
                    color = InputConsoleBox.Colors.DarkPurple;
                    break;
                case LogType.WARNING:
                    color = InputConsoleBox.Colors.DarkYellow;
                    break;
            }

            m_outputBox.WriteLine(msg, color, InputConsoleBox.Colors.Black);
        }
    }
}