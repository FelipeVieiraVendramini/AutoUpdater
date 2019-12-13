using System;

namespace Core.Interfaces
{
    /// <summary>
    /// An asynchronous socket interface encapsulates an asynchronous socket type, either client or server. This
    /// interface allows the passport to be used for server and client socket systems.
    /// </summary>
    public interface IAsynchronousSocket
    {
        string Name { get; set; } // The name of the server.
        int FooterLength { get; set; } // The length of the footer for each packet.
        string Footer { get; set; } // The text for the footer at the end of each packet.
        void Disconnect(IAsyncResult result); // Disconnection method for the client / server.
    }
}
