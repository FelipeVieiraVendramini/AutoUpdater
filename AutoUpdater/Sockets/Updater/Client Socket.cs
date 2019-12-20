#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdater - Client Socket.cs
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
using System.Linq;
using System.Net.Sockets;
using AutoUpdaterCore.Sockets;
using AutoUpdaterCore.Sockets.Packets;
using AutoUpdaterCore.Windows;

namespace AutoUpdater.Sockets.Updater
{
    public sealed class ClientSocket : AsynchronousClientSocket
    {
        readonly PacketProcessor<PacketHandlerType, PacketType, Action<PatchServer, byte[]>> m_pProcessor;

        public ClientSocket()
            : base("", AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            OnClientConnect = Connect;
            OnClientReceive = Receive;
            OnClientDisconnect = Disconnect;

            m_pProcessor = new PacketProcessor<PacketHandlerType, PacketType, Action<PatchServer, byte[]>>(new PatcherPacketHandler());
        }

        /// <summary>
        ///     This method is invoked when the client has been approved of connecting to the server. The client should
        ///     be constructed in this method, and cipher algorithms should be initialized. If any packets need to be
        ///     sent in the connection state, they should be sent here.
        /// </summary>
        /// <param name="pState">Represents the status of an asynchronous operation.</param>
        public void Connect(AsynchronousState pState)
        {
            var pServer = new PatchServer(this, pState.Socket);
            pState.Client = pServer;

            Program.FrmMain.Edit(Program.FrmMain.lblCenterStatus, LabelAsyncOperation.Text, LanguageManager.GetString("StrCheckingPrivacyTerms"));
            
            MsgClientInfo msg = new MsgClientInfo();
            msg.MacAddress = Program.FrmMain.GetMacAddress();
            msg.Append(SystemProperties.GetObjects(MsgClientInfo.OPERATING_SYSTEM, MsgClientInfo.OperatingSystem).Values.ToArray());
            msg.Append(SystemProperties.GetObjects(MsgClientInfo.BASE_BOARD, MsgClientInfo.BaseBoard).Values.ToArray());
            msg.Append(SystemProperties.GetObjects(MsgClientInfo.PROCESSOR, MsgClientInfo.Processor).Values.ToArray());
            msg.Append(SystemProperties.GetObjects(MsgClientInfo.PHYSICAL_MEMORY, MsgClientInfo.PhysicalMemory).Values.ToArray());
            msg.Append(SystemProperties.GetObjects(MsgClientInfo.VIDEO_CONTROLLER, MsgClientInfo.VideoController).Values.ToArray());
            pServer.Send(msg);
        }

        /// <summary>
        ///     This method is invoked when the client has data ready to be processed by the server. The server will
        ///     switch between the packet type to find an appropriate function for processing the packet. If the
        ///     packet was not found, the packet will be outputted to the console as an error.
        /// </summary>
        /// <param name="pState">Represents the status of an asynchronous operation.</param>
        public void Receive(AsynchronousState pState)
        {
            if (pState.Client is PatchServer pServer && pServer.Packet != null)
            {
                var type = (PacketType) BitConverter.ToUInt16(pServer.Packet, 2);
                Action<PatchServer, byte[]> action = m_pProcessor[type];

                // Process the client's packet:
                if (action != null) action(pServer, pServer.Packet);
                else PatcherPacketHandler.Report(pServer.Packet);
            }
        }

        /// <summary>
        ///     This method is invoked when the client is disconnecting from the server. It disconnects the client
        ///     from the map server and disposes of game structures. Upon disconnecting from the map server, the
        ///     character will be removed from the map and screen actions will be terminated. Then, character
        ///     data will be disposed of.
        /// </summary>
        /// <param name="pState">Represents the status of an asynchronous operation.</param>
        public void Disconnect(object pState)
        {
            if (!(pState is PatchServer pObj))
                return;
        }
    }
}