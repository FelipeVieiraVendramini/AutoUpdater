#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - 25000 - MsgRequestInfo.cs
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

namespace AutoUpdaterCore.Sockets.Packets
{
    public class MsgRequestInfo : PacketStructure
    {
        public MsgRequestInfo(byte[] buffer)
            : base(buffer)
        {
        }

        public MsgRequestInfo()
            : base(PacketType.MsgRequestInfo, 8, 8)
        {
        }

        public AutoUpdateRequestType Mode
        {
            get => (AutoUpdateRequestType) ReadUShort(4);
            set => WriteUShort((ushort) value, 4);
        }

        public ushort CurrentVersion
        {
            get => ReadUShort(6);
            set => WriteUShort(value, 6);
        }
    }

    public enum AutoUpdateRequestType : ushort
    {
        CheckForLauncherUpdates, // sent from client
        LauncherUpdatesOk, // sent from server
        CheckForGameUpdates, // sent from client
        GameUpdatesOk // sent from server
    }
}