#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - MsgDownloadInfo.cs
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

using System.Runtime.InteropServices;

namespace AutoUpdaterCore.Sockets.Packets
{
    public class MsgDownloadInfo
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PacketInfo
        {
            public ushort Size;
            public PacketType Type;
        }
    }
}