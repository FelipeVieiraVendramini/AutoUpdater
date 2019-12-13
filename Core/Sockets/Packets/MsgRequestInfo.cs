using System.Runtime.InteropServices;

namespace Core.Sockets.Packets
{
    public partial class MsgRequestInfo : Packet<MsgRequestInfo.PacketInfo>
    {
        public struct PacketInfo
        {
            public ushort Size;
            public PacketType Type;
            public AutoUpdateRequestType Mode;
        }

        public MsgRequestInfo()
        {
            Info.Type = PacketType.MsgRequestInfo;
        }

        public MsgRequestInfo(byte[] msg)
            : base(msg)
        {
        }

        public bool Create(AutoUpdateRequestType type)
        {
            Info.Mode = type;

            Info.Size = (ushort) Marshal.SizeOf(Info);
            return true;
        }
    }

    public enum AutoUpdateRequestType : ushort
    {
        CheckForLauncherUpdates, // sent from client
        CheckForGameUpdates // sent from client
    }
}