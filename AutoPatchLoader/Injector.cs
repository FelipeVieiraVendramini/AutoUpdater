#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoPatchLoader - Injector.cs
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

#region References

using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace AutoPatchLoader
{
    public static class Injector
    {
        /// <summary>
        ///     Fonction qui injecte une dll
        /// </summary>
        /// <param name="DllName">Nom de la dll qui va être injectée.</param>
        /// <param name="ProcessName">Nom du processus dans lequel la dll sera injectée.</param>
        public static bool StartInjection(string DllName, uint ProcessID)
        {
            try
            {
                IntPtr hProcess = new IntPtr(0); //openprocess
                IntPtr hModule = new IntPtr(0); //vritualAllocex
                IntPtr Injector = new IntPtr(0); //getprocadress
                IntPtr hThread = new IntPtr(0); //createremotethread
                int LenWrite = DllName.Length + 1;

                //on ouvre le processus avec tout les droits d'accés
                hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, ProcessID);

                //si il a bien été ouvert
                if (hProcess != IntPtr.Zero)
                {
                    //on va allouer de la mémoire
                    hModule = VirtualAllocEx(hProcess, IntPtr.Zero, (UIntPtr) LenWrite, MEM_COMMIT,
                        PAGE_EXECUTE_READWRITE);

                    //si on a bien alloué de la mémoire
                    if (hModule != IntPtr.Zero)
                    {
                        //on va écrire le nom de la dll dans le process
                        ASCIIEncoding Encoder = new ASCIIEncoding();

                        //nombre de bytes écrits
                        int Written = 0;

                        //si on a bien écrit dans le process
                        if (WriteProcessMemory(hProcess, hModule, Encoder.GetBytes(DllName), LenWrite, Written))
                        {
                            //on va rechercher la fonction LoadLibrary qui va charger la dll
                            Injector = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                            //si on a bien trouvé l'adresse
                            if (Injector != IntPtr.Zero)
                            {
                                //on lance le thread qui va s'executer dans l'espace mémoire du processus et charger la dll
                                hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, Injector, hModule, 0, 0);

                                //pas d'erreur avec le lancement du thread
                                if (hThread != IntPtr.Zero)
                                {
                                    //10 secondes
                                    uint Result = WaitForSingleObject(hThread, 10 * 1000);

                                    //...
                                    if (Result != WAIT_FAILED || Result != WAIT_ABANDONED
                                                              || Result != WAIT_OBJECT_0 || Result != WAIT_TIMEOUT)
                                    {
                                        //on désalloc la mémoire allouée
                                        if (VirtualFreeEx(hProcess, hModule, 0, MEM_RELEASE))
                                        {
                                            //on regarde si l'handle du thread retourné n'est pas null
                                            if (hThread != IntPtr.Zero)
                                            {
                                                //injection réussie :]
                                                CloseHandle(hThread);
                                                return true;
                                            }

                                            throw new Exception("Mauvais Handle du thread...injection échouée");
                                        }

                                        throw new Exception("Problème libèration de mémoire...injection échouée");
                                    }

                                    throw new Exception("WaitForSingle échoué : " + Result + "...injection échouée");
                                }

                                throw new Exception("Problème au lancement du thread...injection échouée");
                            }

                            throw new Exception("Adresse LoadLibraryA non trouvée...injection échouée");
                        }

                        throw new Exception("Erreur d'écriture dans le processus...injection échouée");
                    }

                    throw new Exception("Mémoire non allouée...injection échouée");
                }

                throw new Exception("Processus non ouvert...injection échouée");
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.ToString());
                return false;
            }
        }

        #region Win32

        [DllImport("KERNEL32.DLL")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("KERNEL32.DLL")]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAdress, UIntPtr dwSize,
            uint flAllocationType, uint flProtect);

        [DllImport("KERNEL32.DLL")]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAdress, uint dwSize, uint dwFreeType);

        [DllImport("KERNEL32.DLL")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize,
            int lpNumberOfBytesWritten);

        [DllImport("KERNEL32.DLL")]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr se, uint dwStackSize,
            IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, uint lpThreadId);

        [DllImport("KERNEL32.DLL", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("KERNEL32.DLL", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("KERNEL32.DLL")]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("KERNEL32.DLL")]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliSeconds);

        private const uint PROCESS_ALL_ACCESS = 0x0002 | 0x0400 | 0x0008 | 0x0010 | 0x0020;
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RELEASE = 0x8000;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;
        private const uint WAIT_ABANDONED = 0x00000080;
        private const uint WAIT_OBJECT_0 = 0x00000000;
        private const uint WAIT_TIMEOUT = 0x00000102;
        private const uint WAIT_FAILED = 0xFFFFFFFF;

        #endregion
    }
}
// by cptsky