#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - PatcherPacketHandler.cs
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

namespace AutoUpdater.Sockets.Updater
{
    public class PatcherPacketHandler
    {
        [PacketHandlerType(PacketType.MsgRequestInfo)]
        public void ProcessRequestInfo(PatchServer server, byte[] buffer)
        {
            MsgRequestInfo msg = new MsgRequestInfo(buffer);
            switch (msg.Mode)
            {
                case AutoUpdateRequestType.LauncherUpdatesOk:
                    Kernel.Stage = AutoPatchStage.WaitingForGamePatchs;
                    Program.FrmMain.RequestPatches(AutoUpdateRequestType.CheckForGameUpdates, server);
                    break;
                case AutoUpdateRequestType.GameUpdatesOk:
                    Program.FrmMain.NoDownload(UpdateReturnMessage.Success);
                    break;
            }
        }

        [PacketHandlerType(PacketType.MsgDownloadInfo)]
        public void ProcessDownloadInfo(PatchServer server, byte[] buffer)
        {
            MsgDownloadInfo msg = new MsgDownloadInfo(buffer);
            switch (msg.Mode)
            {
                case UpdateDownloadType.UpdaterPatch:
                    if (Kernel.Stage != AutoPatchStage.WaitingForUpdaterPatchs)
                        return;

                    Program.FrmMain.PrepareToDownload(msg.Mode, msg.GetStrings(), server);
                    break;

                case UpdateDownloadType.GameClientPatch:
                    if (Kernel.Stage != AutoPatchStage.WaitingForGamePatchs)
                        return;

                    Program.FrmMain.PrepareToDownload(msg.Mode, msg.GetStrings(), server);
                    break;
            }
        }

        [PacketHandlerType(PacketType.MsgClientInfo)]
        public void ProcessClientInfo(PatchServer server, byte[] buffer)
        {
            Program.FrmMain.ProcessPacket(server, buffer);
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
            Kernel.Log.WriteToFile($"Missing Packet Handler: {identity} (Length: {length})", "missing_packet");
            Kernel.Log.WriteToFile(PacketDump.Hex(packet), "missing_packet");
        }
    }
}