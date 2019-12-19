#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - LogWriter.cs
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
using System.IO;

namespace AutoUpdaterCore
{
    public class LogWriter
    {
        public const string STR_SYSLOG_FORMAT = "{0} [{1}] - {2}";
        public const string STR_SYSLOG_FOLDER = @"debug\";
        public const string STR_SYSLOG_GAMESERVER = "Updater";

        private readonly string m_szMainDirectory;

        /// <summary>
        ///     Start a new instance and create the necessary folders.
        /// </summary>
        public LogWriter(string szPath)
        {
            if (!szPath.EndsWith("\\"))
                szPath += "\\";

            m_szMainDirectory = szPath;
            CheckFolders();
        }

        /// <summary>
        ///     This method will write the message to the log in the main file and wont show
        ///     it on the console.
        /// </summary>
        /// <param name="szMessage">The message buffer that will be written.</param>
        public string SaveLog(string szMessage)
        {
            return SaveLog(szMessage, LogType.MESSAGE);
        }

        /// <summary>
        ///     This method will write the message to the default file with the required log type.
        /// </summary>
        /// <param name="szMessage">The message that will be written.</param>
        /// <param name="ltLog">The kind of message that will be shown.</param>
        public string SaveLog(string szMessage, LogType ltLog)
        {
            return SaveLog(szMessage, STR_SYSLOG_GAMESERVER, ltLog);
        }

        /// <summary>
        ///     This method should be used when it should not show date time settings.
        /// </summary>
        /// <param name="szMessage"></param>
        /// <param name="szFileName"></param>
        public string SavePureLog(string szMessage, string szFileName)
        {
            CheckFolders();
            string szFilePath = m_szMainDirectory + STR_SYSLOG_FOLDER + szFileName;
            WriteToFile(szMessage = FormatSysString(szMessage, 0, false), szFilePath);
            return szMessage;
        }

        /// <summary>
        ///     This method will write the message on the required file with the defined parameters.
        /// </summary>
        /// <param name="szMessage">The message that will be written.</param>
        /// <param name="szFileName">The file name where the log will be written.</param>
        /// <param name="ltLog">The kind of message that will be shown.</param>
        public string SaveLog(string szMessage, string szFileName, LogType ltLog = LogType.MESSAGE)
        {
            CheckFolders();

            string szDefault = szMessage;
            szMessage = FormatSysString(szMessage, ltLog);

            string szFilePath = m_szMainDirectory + STR_SYSLOG_FOLDER + szFileName;
            WriteToFile(szMessage, szFilePath);
            return szMessage;
        }

        public void WriteToFile(string szFullMessage, string szFilePath)
        {
            bool bStop = false;

            szFilePath = szFilePath + DateTime.Now.ToString("yyyy-M-dd") + ".log";

            if (!File.Exists(szFilePath))
                File.Create(szFilePath).Close();

            while (!bStop)
                try
                {
                    var pWriter = File.AppendText(szFilePath);
                    pWriter.WriteLine(szFullMessage);
                    pWriter.Close();
                    bStop = true;
                }
                catch
                {
                }
        }

        private string FormatSysString(string szMessage, LogType ltType, bool bTime = true)
        {
            if (bTime)
                return string.Format(STR_SYSLOG_FORMAT, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ltType,
                    szMessage);
            return szMessage;
        }

        /// <summary>
        ///     This method will check if the folders are created to avoid a exception while
        ///     writing to the log.
        /// </summary>
        private void CheckFolders()
        {
            try
            {
                if (!Directory.Exists(m_szMainDirectory + STR_SYSLOG_FOLDER))
                    Directory.CreateDirectory(m_szMainDirectory + STR_SYSLOG_FOLDER);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public enum LogType
    {
        MESSAGE,
        DEBUG,
        WARNING,
        ERROR,
        EXCEPTION,
        CONSOLE
    }
}