#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - AsymmetricCryptoHandler.cs
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
using System.Security.Cryptography;

namespace AutoUpdaterCore.Security
{
    public sealed class AsymmetricCryptoHandler
    {
        private bool m_bIsCrypto;
        RSAParameters m_params;

        /// <summary>
        ///     Create a new asymetric
        /// </summary>
        /// <param name="param"></param>
        /// <param name="isCrypto"></param>
        public AsymmetricCryptoHandler(RSAParameters param, bool isCrypto)
        {
            m_bIsCrypto = isCrypto;
            m_params = param;
        }

        public byte[] Encrypt(byte[] data)
        {
            if (!m_bIsCrypto)
                throw new Exception("Using decrypt handler to encrypt data.");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(m_params);
            return rsa.Encrypt(data, false);
        }

        public byte[] Decrypt(byte[] data)
        {
            if (m_bIsCrypto)
                throw new Exception("Using crypto handler to decrypt data.");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(m_params);
            return rsa.Decrypt(data, false);
        }
    }
}