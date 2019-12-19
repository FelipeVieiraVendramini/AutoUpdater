#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchServer - Kernel.cs
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

using AutoUpdaterCore;

namespace AutoPatchServer
{
    public static class Kernel
    {
        public static string DownloadUrl = "https://ftwmasters.com.br/patches";
        public static int ListenPort = 9528;
        public static int LatestUpdaterPatch = 10000;
        public static int LatestGamePatch = 4000;

        public static MyXml MyXml;
    }
}