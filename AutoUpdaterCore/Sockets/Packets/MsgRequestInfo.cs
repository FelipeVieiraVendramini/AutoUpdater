#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - MsgRequestInfo.cs
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
    public class MsgRequestInfo : Packet<MsgRequestInfo.PacketInfo>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PacketInfo
        {
            public ushort Size;
            public PacketType Type;
            public AutoUpdateRequestType Mode;
        }

        public MsgRequestInfo()
        {
            Info.Type = PacketType.MsgRequestInfo;
        }

        public MsgRequestInfo(byte[] msg)
            : base(msg)
        {
        }

        public bool Create(AutoUpdateRequestType type)
        {
            Info.Mode = type;

            Info.Size = (ushort) Marshal.SizeOf(Info);
            return true;
        }
    }

    public enum AutoUpdateRequestType : ushort
    {
        CheckForLauncherUpdates, // sent from client
        CheckForGameUpdates, // sent from client
        GameUpdatesOk // sent from server
    }
}