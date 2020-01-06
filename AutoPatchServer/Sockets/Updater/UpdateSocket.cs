#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchServer - UpdateSocket.cs
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
using System.Net.Sockets;
using AutoUpdaterCore.Sockets;
using AutoUpdaterCore.Sockets.Packets;

namespace AutoPatchServer.Sockets.Updater
{
    public sealed class UpdateSocket : AsynchronousServerSocket
    {
        // Local-Scope Variable Declarations:
        PacketProcessor<PacketHandlerType, PacketType, Action<User, byte[]>> m_processor;

        public UpdateSocket()
            : base("FtwUpdateSocket", AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            OnClientConnect = Connect;
            OnClientReceive = Receive;
            OnClientDisconnect = Disconnect;

            m_processor = new PacketProcessor<PacketHandlerType, PacketType, Action<User, byte[]>>
                (new UpdaterPacketHandler());
        }

        /// <summary>
        ///     This method is invoked when the client has been approved of connecting to the server. The client should
        ///     be constructed in this method, and cipher algorithms should be initialized. If any packets need to be
        ///     sent in the connection state, they should be sent here.
        /// </summary>
        /// <param name="state">Represents the status of an asynchronous operation.</param>
        public void Connect(AsynchronousState state)
        {
            // Create the client for the asynchronous state:
            User client = new User(this, state.Socket, null);
            state.Client = client;
        }

        /// <summary>
        ///     This method is invoked when the client has data ready to be processed by the server. The server will
        ///     switch between the packet type to find an appropriate function for processing the packet. If the
        ///     packet was not found, the packet will be outputted to the console as an error.
        /// </summary>
        /// <param name="state">Represents the status of an asynchronous operation.</param>
        public void Receive(AsynchronousState state)
        {
            User client = state.Client as User;
            if (client?.Packet != null)
            {
                // Get the packet handler from the packet processor:
                PacketType type = (PacketType) BitConverter.ToUInt16(client.Packet, 2);
                Action<User, byte[]> action = m_processor[type];

                // Process the client's packet:
                if (action != null) action(client, client.Packet);
                else UpdaterPacketHandler.Report(client.Packet);
            }
        }

        /// <summary>
        ///     This method is invoked when the client is disconnecting from the server. It disconnects the client
        ///     from the map server and disposes of game structures. Upon disconnecting from the map server, the
        ///     character will be removed from the map and screen actions will be terminated. Then, character
        ///     data will be disposed of.
        /// </summary>
        /// <param name="state">Represents the status of an asynchronous operation.</param>
        public void Disconnect(object state)
        {
            if (!(state is User client))
                return;

            Kernel.AllowedUsers.TryRemove(client.MacAddress, out _);
        }
    }
}