using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Sockets.Packets
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
    }
}