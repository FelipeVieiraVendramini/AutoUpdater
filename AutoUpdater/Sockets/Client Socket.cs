using System;
using System.Net.Sockets;
using AutoUpdaterCore.Sockets;
using AutoUpdaterCore.Sockets.Packets;

namespace AutoUpdater.Sockets
{
    public sealed class ClientSocket : AsynchronousClientSocket
    {
        public ClientSocket()
            : base("", AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            OnClientConnect = Connect;
            OnClientReceive = Receive;
            OnClientDisconnect = Disconnect;
        }

        /// <summary>
        /// This method is invoked when the client has been approved of connecting to the server. The client should
        /// be constructed in this method, and cipher algorithms should be initialized. If any packets need to be
        /// sent in the connection state, they should be sent here.
        /// </summary>
        /// <param name="pState">Represents the status of an asynchronous operation.</param>
        public void Connect(AsynchronousState pState)
        {
            var pServer = new PatchServer(this, pState.Socket);
            pState.Client = pServer;
        }

        /// <summary>
        /// This method is invoked when the client has data ready to be processed by the server. The server will
        /// switch between the packet type to find an appropriate function for processing the packet. If the
        /// packet was not found, the packet will be outputted to the console as an error.
        /// </summary>
        /// <param name="pState">Represents the status of an asynchronous operation.</param>
        public void Receive(AsynchronousState pState)
        {
            if (pState.Client is PatchServer pServer && pServer.Packet != null)
            {
                var type = (PacketType)BitConverter.ToUInt16(pServer.Packet, 2);
                switch (type)
                {
                    case PacketType.MsgRequestInfo:
                        MsgRequestInfo msg = new MsgRequestInfo(pServer.Packet);
                        msg.Process(pServer);
                        break;
                }
            }
        }

        /// <summary>
        /// This method is invoked when the client is disconnecting from the server. It disconnects the client
        /// from the map server and disposes of game structures. Upon disconnecting from the map server, the
        /// character will be removed from the map and screen actions will be terminated. Then, character
        /// data will be disposed of.
        /// </summary>
        /// <param name="pState">Represents the status of an asynchronous operation.</param>
        public void Disconnect(object pState)
        {
            var pObj = pState as PatchServer;
            if (pObj == null)
            {

            }
        }
    }
}
