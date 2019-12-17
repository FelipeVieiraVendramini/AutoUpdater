#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - IAsynchronousSocket.cs
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

namespace AutoUpdaterCore.Interfaces
{
    /// <summary>
    ///     An asynchronous socket interface encapsulates an asynchronous socket type, either client or server. This
    ///     interface allows the passport to be used for server and client socket systems.
    /// </summary>
    public interface IAsynchronousSocket
    {
        string Name { get; set; } // The name of the server.
        int FooterLength { get; set; } // The length of the footer for each packet.
        string Footer { get; set; } // The text for the footer at the end of each packet.
        void Disconnect(IAsyncResult result); // Disconnection method for the client / server.
    }
}