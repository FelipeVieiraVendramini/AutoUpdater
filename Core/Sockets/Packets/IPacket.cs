using System;
using System.Runtime.InteropServices;

namespace Core.Sockets.Packets
{
    public abstract class Packet<TE> where TE : struct
    {
        public TE Info;

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
            int len = Marshal.SizeOf(Info);
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