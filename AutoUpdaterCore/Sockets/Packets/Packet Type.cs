#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - Packet Type.cs
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
    /// <summary>
    ///     This structure encapsulates a packet's header information. It contains the length of the packet in offset
    ///     zero as an unsigned short, and the identity of the packet in offset 2 as an unsigned short.
    /// </summary>
    public struct PacketHeader
    {
        public ushort Length;
        public PacketType Identity;
    }

    public enum PacketType : ushort
    {
        MsgRequestInfo = 25000,
        MsgDownloadInfo = 25001,
        MsgClientInfo = 25002
    }
}