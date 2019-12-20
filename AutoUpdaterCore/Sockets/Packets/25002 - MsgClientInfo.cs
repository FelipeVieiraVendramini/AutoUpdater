#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - 25002 - MsgClientInfo.cs
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

using System.Collections.Generic;

namespace AutoUpdaterCore.Sockets.Packets
{
    public sealed class MsgClientInfo : PacketStructure
    {
        public const string OPERATING_SYSTEM = "Win32_OperatingSystem";
        public const string BASE_BOARD = "Win32_BaseBoard";
        public const string PROCESSOR = "Win32_Processor";
        public const string PHYSICAL_MEMORY = "Win32_PhysicalMemory";
        public const string VIDEO_CONTROLLER = "Win32_VideoController";

        public static string[] OperatingSystem =
        {
            "Manufacturer",
            "Caption",
            "OSArchiteture",
            "MUILanguages"
        };

        public static string[] BaseBoard =
        {
            "Manufacturer",
            "Product"
        };

        public static string[] Processor =
        {
            "SystemName",
            "Name",
            "Manufacturer",
            "NumberOfCores"
        };

        public static string[] PhysicalMemory =
        {
            "ConfiguredClockSpeed",
            "Capacity",
            "DeviceLocator"
        };

        public static string[] VideoController =
        {
            "Caption",
            "VideoProcessor",
            "CurrentHorizontalResolution",
            "CurrentVerticalResolution",
            "VideoModeDescription",
            "MinRefreshRate",
            "MaxRefreshRate"
        };

        private const int _STR_OFFSET = 20;
        private readonly StringPacker m_packer = new StringPacker();

        public MsgClientInfo()
            : base(PacketType.MsgClientInfo, 22, 22)
        {
        }

        public MsgClientInfo(byte[] buffer)
            : base(buffer)
        {
            m_packer = new StringPacker(buffer, _STR_OFFSET);
        }

        public string MacAddress
        {
            get => ReadString(16, 4);
            set => WriteString(value, 16, 4);
        }

        public void Append(params string[] strs)
        {
            m_packer.Add(strs);
        }

        public List<string> GetStrings()
        {
            return m_packer.GetStrings();
        }

        protected override byte[] Build()
        {
            byte[] pStr = m_packer.ToArray();
            Resize(22 + pStr.Length);
            WriteHeader(Length, PacketType.MsgClientInfo);
            WriteArray(pStr, pStr.Length, _STR_OFFSET);
            return base.Build();
        }
    }
}