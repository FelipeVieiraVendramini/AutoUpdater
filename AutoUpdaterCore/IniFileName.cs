#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - IniFileName.cs
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
using System.Text;

namespace AutoUpdaterCore
{ // Set the IniFileName class within the INI Namespace.
    public class IniFileName
    {
        //	Imports the Win32 Function "GetPrivateProfileString" from the "Kernel32" class.
        //	I use 3 methods to gather the information. All have the same name as defind by the Win32 Function "GetPrivateProfileString"
        //
        //	First Method, Gathers the Value, as the SectionHeader and EntryKey are know.
        //
        //	Second Method, Gathers a list of EntryKey for the known SectionHeader 
        //
        //	Third Method, Gathers a list of SectionHeaders.


        // First Method
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Value, StringBuilder Result,
            int Size, string FileName);

        // Second Method
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, int Key, string Value,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int Size, string FileName);

        // Third Method
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key, string Value,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int Size, string FileName);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // Set the IniFileName passed from the Main Application.
        /// <summary>
        ///     The path of the ini file.
        /// </summary>
        public string Path;

        /// <summary>
        ///     Initialize the Ini Parser. If the param is null, it will read the file
        ///     Shell.ini on the root folder, then, if not found, it will just thrown a
        ///     non-handled exception.
        /// </summary>
        /// <param name="iniPath">The path where the file to be read is located.</param>
        public IniFileName(string iniPath)
        {
            if (iniPath == null) Path = Environment.CurrentDirectory + @"\Shell.ini";
            Path = iniPath;
        }

        // The Function called to obtain the SectionHeaders, and returns them in an Dynamic Array.
        /// <summary>
        ///     This function will obtain all Section Headers.
        /// </summary>
        /// <returns>Will return an array with all Headers on the file.</returns>
        public string[] GetSectionNames()
        {
            //	Sets the maxsize buffer to 500, if the more is required then doubles the size each time.
            for (int maxsize = 500;; maxsize *= 2)
            {
                //	Obtains the information in bytes and stores them in the maxsize buffer (Bytes array)
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(0, "", "", bytes, maxsize, Path);

                // Check the information obtained is not bigger than the allocated maxsize buffer - 2 bytes.
                // if it is, then skip over the next section so that the maxsize buffer can be doubled.
                if (size < maxsize - 2)
                {
                    // Converts the bytes value into an ASCII char. This is one long string.
                    string Selected = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    // Splits the Long string into an array based on the "\0" or null (Newline) value and returns the value(s) in an array
                    return Selected.Split('\0');
                }
            }
        }

        // The Function called to obtain the EntryKey's from the given SectionHeader string passed and returns them in an Dynamic Array
        /// <summary>
        ///     This function will get all Entries on a [Section].
        /// </summary>
        /// <param name="section">The name of the section you want all params.</param>
        /// <returns>Returns an array with all Entries on the file.</returns>
        public string[] GetEntryNames(string section)
        {
            //	Sets the maxsize buffer to 500, if the more is required then doubles the size each time. 
            for (int maxsize = 500;; maxsize *= 2)
            {
                //	Obtains the EntryKey information in bytes and stores them in the maxsize buffer (Bytes array).
                //	Note that the SectionHeader value has been passed.
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(section, 0, "", bytes, maxsize, Path);

                // Check the information obtained is not bigger than the allocated maxsize buffer - 2 bytes.
                // if it is, then skip over the next section so that the maxsize buffer can be doubled.
                if (size < maxsize - 2)
                {
                    // Converts the bytes value into an ASCII char. This is one long string.
                    string entries = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    // Splits the Long string into an array based on the "\0" or null (Newline) value and returns the value(s) in an array
                    return entries.Split('\0');
                }
            }
        }

        // The Function called to obtain the EntryKey Value from the given SectionHeader and EntryKey string passed, then returned
        /// <summary>
        ///     Gets the value set on the [Section] and Entry.
        /// </summary>
        /// <param name="section">The [Section] you want to check.</param>
        /// <param name="entry">The Entry you want to get the value.</param>
        /// <returns>The value of the config you'd set.</returns>
        public object GetEntryValue(string section, string entry)
        {
            //	Sets the maxsize buffer to 250, if the more is required then doubles the size each time. 
            for (int maxsize = 250;; maxsize *= 2)
            {
                //	Obtains the EntryValue information and uses the StringBuilder Function to and stores them in the maxsize buffers (result).
                //	Note that the SectionHeader and EntryKey values has been passed.
                StringBuilder result = new StringBuilder(maxsize);
                int size = GetPrivateProfileString(section, entry, "", result, maxsize, Path);
                if (size < maxsize - 1)
                    // Returns the value gathered from the EntryKey
                    return result.ToString();
            }
        }

        public bool SetValue(string section, string entry, object value)
        {
            try
            {
                long ret = 0;
                if (value == null)
                    ret = WritePrivateProfileString(section, entry, null, Path);
                else
                    ret = WritePrivateProfileString(section, entry, value.ToString(), Path);
                return ret > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}