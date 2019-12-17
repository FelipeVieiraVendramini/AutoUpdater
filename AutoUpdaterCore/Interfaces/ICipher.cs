#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - ICipher.cs
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

namespace AutoUpdaterCore.Interfaces
{
    /// <summary>
    ///     This interface is used to define a cipher for packet processing. The socket systems use this interface to
    ///     decrypt the header and body of packets. All packet ciphers should implement this method.
    /// </summary>
    public interface ICipher
    {
        byte[] Decrypt(byte[] buffer, int length);
        void Decrypt(byte[] packet, byte[] buffer, int length, int position);
        byte[] Encrypt(byte[] packet, int length);
        void GenerateKeys(int account, int authentication);
        void KeySchedule(byte[] key);
    }
}