#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchServer - User.cs
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
using AutoUpdaterCore.Interfaces;
using AutoUpdaterCore.Sockets;

namespace AutoPatchServer.Sockets.Updater
{
    public sealed class User : Passport
    {
        public string MacAddress = "000000000000";

        private AsynchronousServerSocket m_socket;

        public User(AsynchronousServerSocket server, Socket socket, ICipher cipher)
            : base(server, socket, cipher)
        {
            m_socket = server;
        }

        // todo improve ban system allowing ranges :)
        public bool IsBanned => Kernel.BannedIpAddresses.Contains(IpAddress) || Kernel.BannedMacAddresses.Contains(MacAddress);
    }
}