#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - MyTests - Program.cs
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
using AutoUpdaterCore.Sockets;
using AutoUpdaterCore.Sockets.Packets;

namespace MyTests
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

            StringPacker packer = new StringPacker("Testando", "Aurelio", "Felipe", "Teste", "StringPacker", "FTW! Masters");
            Console.WriteLine(PacketDump.Hex(packer.ToArray()));

            packer = new StringPacker(packer.ToArray());
            foreach (var str in packer.GetStrings())
                Console.WriteLine(str);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}