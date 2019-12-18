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

using AutoUpdaterCore.Windows;

namespace AutoPatchServer
{
    class Program
    {
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
        }
    }
}