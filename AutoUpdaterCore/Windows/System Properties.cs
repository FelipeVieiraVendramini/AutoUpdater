#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - System Properties.cs
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
using System.Management;

namespace AutoUpdaterCore.Windows
{
    public static class SystemProperties
    {
        public static Dictionary<string, string> GetObjects(string key, params string[] names)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
            try
            {
                foreach (ManagementObject share in searcher.Get())
                {
                    if (share.Properties.Count <= 0) return new Dictionary<string, string>();

                    foreach (PropertyData PC in share.Properties)
                    {
                        if (names.All(x => x != PC.Name))
                            continue;

                        if (PC.Value != null && PC.Value.ToString() != "")
                            switch (PC.Value.GetType().ToString())
                            {
                                case "System.String[]":
                                    string[] str = (string[]) PC.Value;
                                    string str2 = "";
                                    foreach (string st in str)
                                        str2 += st + " ";
                                    result.Add(PC.Name, str2);
                                    break;
                                case "System.UInt16[]":
                                    ushort[] shortData = (ushort[]) PC.Value;
                                    string tstr2 = "";
                                    foreach (ushort st in shortData)
                                        tstr2 += st + " ";
                                    result.Add(PC.Name, tstr2);
                                    break;
                                default:
                                    result.Add(PC.Name, PC.Value.ToString());
                                    break;
                            }
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}