#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchServer - Updater Packet Handler.cs
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
using AutoUpdaterCore.Sockets.Packets;

namespace AutoPatchServer.Sockets.Updater
{
    public class UpdaterPacketHandler
    {
        [PacketHandlerType(PacketType.MsgRequestInfo)]
        public void ProcessRequestInfo(User user, byte[] buffer)
        {

        }

        [PacketHandlerType(PacketType.MsgDownloadInfo)]
        public void ProcessDownloadInfo(User user, byte[] buffer)
        {

        }

        /// <summary>
        ///     This function reports a missing packet handler to the console. It writes the length and type of the
        ///     packet, then a packet dump to the console.
        /// </summary>
        /// <param name="packet">The packet buffer being reported.</param>
        public static void Report(byte[] packet)
        {
            ushort length = BitConverter.ToUInt16(packet, 0);
            ushort identity = BitConverter.ToUInt16(packet, 2);

            // Print the packet and the packet header:
            Program.Log.WriteToFile($"Missing Packet Handler: {identity} (Length: {length})", "missing_packet");
            Program.Log.WriteToFile(PacketDump.Hex(packet), "missing_packet");
        }
    }
}