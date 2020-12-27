#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - Program.cs
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
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdater
{
    static class Program
    {
        public static FrmMain FrmMain;

        /// <summary>
        ///     Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
#if !DEBUG
            Mutex appSingleton = new Mutex(false, "MyCQAutoPatchClient");
            try
            {
                if (appSingleton.WaitOne(0, false))
                {
#else
            Environment.CurrentDirectory = @"D:\World Conquer\Client";
#endif
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(FrmMain = new FrmMain());
#if !DEBUG
                }
                else
                {
                    MessageBox.Show(@"You cannot run multiple instances of this program.");
                }
            }
            finally
            {
                appSingleton.ReleaseMutex();
                appSingleton.Close();
                appSingleton.Dispose();
            }
#endif
        }
    }
}