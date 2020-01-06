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

#region References

using System;
using System.Collections.Generic;
using AutoUpdaterCore.Interfaces;
using AutoUpdaterCore.Sockets.Packets;

#endregion

namespace AutoPatchServer.Sockets.Updater
{
    public class UpdaterPacketHandler
    {
        [PacketHandlerType(PacketType.MsgRequestInfo)]
        public void ProcessRequestInfo(User user, byte[] buffer)
        {
            MsgRequestInfo msg = new MsgRequestInfo(buffer);

            if (user.IsBanned)
            {
                msg.CurrentVersion = ushort.MaxValue;
                user.Send(msg);
                return;
            }

            List<PatchStructure> updates = UpdatesManager.GetDownloadList(msg.CurrentVersion);
            switch (msg.Mode)
            {
                case AutoUpdateRequestType.CheckForLauncherUpdates:
                    if (updates.Count > 0)
                    {
                        MsgDownloadInfo pMsg = new MsgDownloadInfo
                        {
                            Mode = UpdateDownloadType.UpdaterPatch,
                            LatestVersion = (ushort) Kernel.LatestUpdaterPatch
                        };
                        pMsg.Append(Kernel.DownloadUrl);
                        foreach (var patch in updates)
                        {
                            pMsg.Append(patch.FileName);
                        }

                        user.Send(pMsg);
                    }
                    else
                    {
                        msg.Mode = AutoUpdateRequestType.LauncherUpdatesOk;
                        user.Send(msg);
                    }

                    break;
                case AutoUpdateRequestType.CheckForGameUpdates:
                    if (updates.Count > 0)
                    {
                        MsgDownloadInfo pMsg = new MsgDownloadInfo
                        {
                            Mode = UpdateDownloadType.GameClientPatch,
                            LatestVersion = (ushort) Kernel.LatestGamePatch
                        };
                        pMsg.Append(Kernel.DownloadUrl);
                        foreach (var patch in updates)
                        {
                            pMsg.Append(patch.FileName);
                        }

                        user.Send(pMsg);
                    }
                    else
                    {
                        msg.Mode = AutoUpdateRequestType.GameUpdatesOk;
                        user.Send(msg);
                    }

                    break;
            }
        }

        [PacketHandlerType(PacketType.MsgDownloadInfo)]
        public void ProcessDownloadInfo(User user, byte[] buffer)
        {
            /**
             * Should not be sent back. Just ignore.
             */
        }

        [PacketHandlerType(PacketType.MsgClientInfo)]
        public void ProcessClientInfo(User user, byte[] buffer)
        {
            /**
             * Todo store user data securely
             * Todo develop something to encrypt data
             */
            MsgClientInfo msg = new MsgClientInfo(buffer);
            Program.WriteLog($"IPAddress [{user.IpAddress}] has connected [MacAddress:{msg.MacAddress}]");

            /**
             * Todo Send user info and Mac Address to the login server to allow connections.
             */

            user.MacAddress = msg.MacAddress;
            /**
             * Sends the latest update! Since it's web host we wont have problems with this. The client
             * will just display the page! :D
             */
            MsgClientInfo back = new MsgClientInfo();
            back.MacAddress = msg.MacAddress;
            back.Append(Kernel.PrivacyTermsUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
            user.Send(back);
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