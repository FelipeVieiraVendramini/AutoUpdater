#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - Packet Attribute.cs
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

namespace AutoUpdaterCore.Sockets.Packets
{
    /// <summary>
    ///     This attribute class provides the server with an associating declarative for packet handlers. Any packet
    ///     handling method declared using this attribute will be added to the packet processor (the red-black tree
    ///     used for storing packet handlers by packet identity).
    /// </summary>
    public sealed class PacketHandlerType : Attribute, IPacketAttribute
    {
        /// <summary>
        ///     This attribute class provides the server with an associating declarative for packet handlers. Any packet
        ///     handling method declared using this attribute will be added to the packet processor (the red-black tree
        ///     used for storing packet handlers by packet identity).
        /// </summary>
        /// <param name="packetType">The type of packet the handler will process.</param>
        public PacketHandlerType(PacketType packetType)
        {
            Type = packetType;
        }

        public IComparable Type { get; set; }
    }

    /// <summary> This interface defines a packet attribute class for the packet processor class. </summary>
    public interface IPacketAttribute
    {
        IComparable Type { get; set; }
    }
}