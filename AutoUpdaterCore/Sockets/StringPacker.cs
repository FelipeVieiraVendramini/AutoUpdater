#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - StringPacker.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoUpdaterCore.Sockets
{
    public class StringPacker
    {
        private List<string> m_lStrings = new List<string>();

        public StringPacker(params string[] strs)
        {
            m_lStrings.AddRange(strs);
        }

        /// <summary>
        ///     Convert an byte array and collects all strings.
        /// </summary>
        /// <param name="msg">The buffer containing all strings.</param>
        /// <param name="startOffset">Just change it if you're inserting the entire packet and the strings starts somewhere.</param>
        public StringPacker(byte[] msg, int startOffset = 0)
        {
            if (msg.Length < 1 || startOffset > msg.Length)
                return;

            int count = msg[startOffset];
            if (count <= 0)
                return;

            int offset = startOffset + 1;
            for (int i = 0; i < count; i++)
            {
                int length = msg[offset++];
                m_lStrings.Add(Encoding.UTF8.GetString(msg, offset, length));
                offset += length;
            }
        }

        public void Add(params string[] strs)
        {
            m_lStrings.AddRange(m_lStrings);
        }

        public byte[] ToArray()
        {
            byte[] msg = new byte[m_lStrings.Count + m_lStrings.Sum(x => x.Length) + 1];
            msg[0] = (byte) m_lStrings.Count;

            int offset = 1;
            foreach (var str in m_lStrings)
            {
                msg[offset++] = (byte) str.Length;
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(str), 0, msg, offset, str.Length);
                offset += str.Length;
            }

            return msg;
        }

        public List<string> GetStrings()
        {
            return m_lStrings.ToList();
        }
    }
}