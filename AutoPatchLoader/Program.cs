#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchLoader - Program.cs
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

namespace AutoPatchLoader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args[0] != "whitenull")
                    throw new Exception("No parameter passed.");

                Process game = new Process();
                game.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                game.StartInfo.FileName = Environment.CurrentDirectory + @"\Conquer.exe";
                game.StartInfo.Arguments = "blacknull";
                game.Start();
                uint idProcess = (uint)game.Id;
                game.Close();
                Injector.StartInjection(Environment.CurrentDirectory + @"\Loader.dll", idProcess);
                Environment.Exit((int) idProcess);
            }
            catch
            {
                Environment.Exit(0);
            }
        }
    }
}