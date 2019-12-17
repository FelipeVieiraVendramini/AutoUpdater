#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - State.cs
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

using System.Net.Sockets;
using System.Threading;

namespace AutoUpdaterCore.Sockets
{
    /// <summary>
    ///     This class encapsulates the asynchronous state from resulting asynchronous operations. It contains the
    ///     player's client, packet buffer, and socket connection.
    /// </summary>
    public sealed class AsynchronousState
    {
        public object Client; // The client objected used in managing the player's instantiated game structures.
        public byte[] Buffer; // The client's packet buffer, used in managing packets.
        public Socket Socket; // The client's remote socket on the server.
        public ManualResetEvent Event; // A manual reset event for waiting on data.

        /// <summary>
        ///     This class encapsulates the asynchronous state from resulting asynchronous operations. It contains the
        ///     player's client, packet buffer, and socket connection.
        /// </summary>
        /// <param name="socket">The client's remote socket on the server.</param>
        public AsynchronousState(Socket socket)
        {
            Client = null;
            Buffer = null;
            Socket = socket;
            Event = new ManualResetEvent(false);
        }
    }
}