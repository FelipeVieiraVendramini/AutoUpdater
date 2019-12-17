#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - Packet.cs
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
using System.Runtime.InteropServices;

namespace AutoUpdaterCore.Sockets.Packets
{
    public abstract class Packet<TE> where TE : struct
    {
        public TE Info;
        protected StringPacker m_packer = null;

        protected Packet()
        {
        }

        protected Packet(byte[] message)
        {
            Decode(message);
        }

        public virtual void Process(Passport client)
        {
            throw new Exception($"There is now handler set for packet {GetType().FullName}");
        }

        protected byte[] Build()
        {
            int len = Marshal.SizeOf(Info);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(Info, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        protected void Decode(byte[] msg)
        {
            int len = msg.Length;
            IntPtr i = Marshal.AllocHGlobal(len);
            Marshal.Copy(msg, 0, i, len);
            Info = (TE) Marshal.PtrToStructure(i, Info.GetType());
            Marshal.FreeHGlobal(i);
        }

        public static implicit operator byte[](Packet<TE> self)
        {
            return self.Build();
        }
    }
}