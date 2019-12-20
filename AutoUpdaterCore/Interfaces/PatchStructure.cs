#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - PatchStructure.cs
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
    public class PatchStructure
    {
        private string m_szName = "";

        public int Order { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public string FileName
        {
            get => string.IsNullOrEmpty(m_szName) ? "" : m_szName.Contains(".exe") ? m_szName : $"{m_szName}.exe";
            set => m_szName = value;
        }
        public bool IsGameUpdate { get; set; }
    }
}