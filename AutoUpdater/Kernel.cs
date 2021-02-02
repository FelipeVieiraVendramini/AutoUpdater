#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - Kernel.cs
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
#if !DEBUG
using System.Reflection;
#endif
using System.Windows.Forms;
using AutoUpdater.Screen;
using AutoUpdaterCore;

namespace AutoUpdater
{
    public static class Kernel
    {
        public static ushort ActualVersion = 1000;
        public static string Version;

        static Kernel()
        {
#if DEBUG
            Version = Application.ProductVersion;
#else
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#endif
            Log = new LogWriter(Environment.CurrentDirectory);
        }

        public static LogWriter Log;

        public static bool HasAgreedPrivacy = false;

        public static AutoPatchStage Stage = AutoPatchStage.None;

        public static void SetClientConfiguration(int width, int height, bool fullScreen, int fpsMode)
        {
            IniFileName ini = new IniFileName(Environment.CurrentDirectory + @"\ini\GameSetUp.ini");
            ini.SetValue("ScreenMode", "ScreenModeRecord", 2);

            ini = new IniFileName(Environment.CurrentDirectory + @"\Config.ini");
            ini.SetValue("GameResolution", "Width", width);
            ini.SetValue("GameResolution", "Height", height);
            
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
            ini.SetValue("0-174", "y", height - 65 - 260);

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
            ini.SetValue("0-339", "x", ((width - 1024) / 2) + 82 + 202);
            ini.SetValue("0-339", "y", height - 115);

            // My Talisman UI
            //ini.SetValue("0-345", "x", 9999);
            // Target Talisman UI
            //ini.SetValue("0-346", "x", 9999);

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
            ini.SetValue("0-3", "x", ((width - 1024) / 2) + 82 + 530);
            ini.SetValue("0-3", "y", height - 115);

            // PKModes
            ini.SetValue("0-191", "x", ((width - 1024) / 2) + 82 + 500);
            ini.SetValue("0-191", "y", height - 100);

            // Map Mini
            ini.SetValue("0-1199", "x", width - 40);

            // Map Full
            ini.SetValue("0-1200", "x", width - 20);

            // Arena
            ini.SetValue("0-403", "x", ((width - 1024) / 2) + 82 + 250);
            ini.SetValue("0-403", "y", height - 115);
            // Action Express Interact Combobox
            ini.SetValue("0-367", "x", ((width - 1024) / 2) + 82 + 350);
            ini.SetValue("0-367", "y", height - 245);
            // Action
            ini.SetValue("0-140", "x", ((width - 1024) / 2) + 82 + 350);
            ini.SetValue("0-140", "y", height - 225);
            // Express
            ini.SetValue("0-274", "x", ((width - 1024) / 2) + 82 + 350);
            ini.SetValue("0-274", "y", height - 215);
            // Interact
            ini.SetValue("0-360", "x", ((width - 1024) / 2) + 82 + 350);
            ini.SetValue("0-360", "y", height - 215);

            // Group UI
            ini.SetValue("0-371", "x", ((width - 1024) / 2) + 82 + 600);
            ini.SetValue("0-371", "y", height - 140);

            // Message Channel
            ini.SetValue("0-357", "y", height - 452);

            // Message Scrollbar
            ini.SetValue("0-1198", "y", height - 477);

            // Mount Vigor
            ini.SetValue("0-383", "x", width - 200);
            ini.SetValue("0-383", "y", height - 200);

            // Target Talisman UI
            ini.SetValue("0-346", "x", width - 345);

            // Arena qualifier main ui
            ini.SetValue("0-402", "x", (width - 665) / 2);
            ini.SetValue("0-402", "y", (height - 560) / 2);
            // Arena qualifier opponent info
            ini.SetValue("0-409", "x", (width - 290) / 2);
            ini.SetValue("0-409", "y", (height - 120) / 2);
            // Arena qualifier fighting points info
            ini.SetValue("0-411", "x", (width - 310) / 2);
            // Arena qualifier quit button
            ini.SetValue("0-412", "x", ((width - 1024) / 2) + 82 + 320);
            ini.SetValue("0-412", "y", height - 130);
            // Arena qualifier count down box
            ini.SetValue("0-407", "x", (width - 290) / 2);
            ini.SetValue("0-407", "y", (height - 110) / 2);
            // Arena qualifier result box 
            ini.SetValue("0-408", "x", (width - 300) / 2);
            ini.SetValue("0-408", "y", (height - 150) / 2);

            ini = new IniFileName(Environment.CurrentDirectory + @"\ini\info.ini");
            // Exp
            ini.SetValue("ExpShowPos", "Exp_XPos", (width / 2) - 100);
            ini.SetValue("ExpShowPos", "Exp_YPos", height - 90);

            // AddExp
            ini.SetValue("ExpShowPos", "AddExp_XPos", (width / 2) - 100 + 90);
            ini.SetValue("ExpShowPos", "AddExp_YPos", height - 90);
        }

        public static ScreenInfo GetClientConfiguration()
        {
            IniFileName ini = new IniFileName(Environment.CurrentDirectory + @"\Config.ini");
            if (!int.TryParse(ini.GetEntryValue("GameResolution", "Width").ToString(), out var width))
                return default;
            if (!int.TryParse(ini.GetEntryValue("GameResolution", "Height").ToString(), out var height))
                return default;
            if (!int.TryParse(ini.GetEntryValue("GameSetup", "FpsMode").ToString(), out var fps))
                return default;
            return new ScreenInfo
            {
                Width = width,
                Height = height,
                FpsMode = fps
            };
        }
    }

    public enum AutoPatchStage
    {
        None,
        WaitingForUpdaterPatchs,
        WaitingForGamePatchs,
        UpdatesOk
    }
}