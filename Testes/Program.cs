using System;
using Core.Sockets.Packets;

namespace Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            MsgRequestInfo msg = new MsgRequestInfo();
            msg.Create(AutoUpdateRequestType.CheckForGameUpdates);
            Console.WriteLine(PacketDump.Hex(msg));

            MsgRequestInfo decode = new MsgRequestInfo(msg);
            Console.WriteLine(PacketDump.Hex(msg));

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}