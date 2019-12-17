#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - CryptoConfig - FrmMain.cs
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
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using AutoUpdaterCore;
using AutoUpdaterCore.Security;

namespace CryptoConfig
{
    public partial class FrmMain : Form
    {
        private const string _KEYS_FILENAME = "config.ini";

        private RSAParameters m_private;
        private RSAParameters m_public;

        private string m_szFilename = string.Empty;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            string dir = Environment.CurrentDirectory + @"\" + _KEYS_FILENAME;

            if (!File.Exists(dir))
                btnGenerateKeys_Click(sender, e);

            IniFileName ini = new IniFileName(dir);
            m_public = new RSAParameters
            {
                Modulus = Convert.FromBase64String(ini.GetEntryValue("Public", "Modulus").ToString()),
                Exponent = Convert.FromBase64String(ini.GetEntryValue("Public", "Exponent").ToString())
            };

            m_private = new RSAParameters
            {
                Modulus = Convert.FromBase64String(ini.GetEntryValue("Private", "Modulus").ToString()),
                Exponent = Convert.FromBase64String(ini.GetEntryValue("Private", "Exponent").ToString()),
                P = Convert.FromBase64String(ini.GetEntryValue("Private", "P").ToString()),
                Q = Convert.FromBase64String(ini.GetEntryValue("Private", "Q").ToString()),
                DP = Convert.FromBase64String(ini.GetEntryValue("Private", "DP").ToString()),
                DQ = Convert.FromBase64String(ini.GetEntryValue("Private", "DQ").ToString()),
                InverseQ = Convert.FromBase64String(ini.GetEntryValue("Private", "Inverse").ToString()),
                D = Convert.FromBase64String(ini.GetEntryValue("Private", "D").ToString())
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                AddExtension = true,
                Filter = @".dat encrypted file|*.dat",
                Title = @"Save your encrypted data to..."
            };

            if (!string.IsNullOrEmpty(m_szFilename))
                dialog.InitialDirectory = m_szFilename;
            else
                dialog.InitialDirectory = Environment.CurrentDirectory;

            if (dialog.ShowDialog() == DialogResult.OK
                && dialog.FileName != string.Empty)
            {
                FileStream fs = new FileStream(dialog.FileName, FileMode.Create);
                byte[] data = Encoding.ASCII.GetBytes(input.Text);
                SymmetricCryptoHandler symmetricCrypto = new SymmetricCryptoHandler(m_private);
                byte[] encrypted = symmetricCrypto.Encrypt(data);

                fs.Write(encrypted, 0, encrypted.Length);

                fs.Close();
                fs.Dispose();
                fs = null;
            }
        }

        private void input_TextChanged(object sender, EventArgs e)
        {
            /*
             *  [AccountServer]
                IpAddress=127.0.0.1
                Port=9958
             */
            byte[] data = Encoding.ASCII.GetBytes(input.Text);
            SymmetricCryptoHandler symmetricCrypto = new SymmetricCryptoHandler(m_private);
            byte[] encrypted = symmetricCrypto.Encrypt(data);
            output.Text = Encoding.ASCII.GetString(encrypted);

            label1.Text = $@"{input.TextLength}:{output.TextLength}";
        }

        private void btnGenerateKeys_Click(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory + @"\" + _KEYS_FILENAME;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                StreamWriter file = new StreamWriter(path, false);
                file.WriteLine("; the asymmetric encryption will be used to store the key and iv of the");
                file.WriteLine("; symmetric protocol.");
                file.WriteLine("");
                file.WriteLine("; used for encryption");
                file.WriteLine("[Public]");
                file.WriteLine("Modulus=");
                file.WriteLine("Exponent=");
                file.WriteLine("");
                file.WriteLine("; used for decryption");
                file.WriteLine("[Private]");
                file.WriteLine("Modulus=");
                file.WriteLine("Exponent=");
                file.WriteLine("P=");
                file.WriteLine("Q=");
                file.WriteLine("DP=");
                file.WriteLine("DQ=");
                file.WriteLine("Inverse=");
                file.WriteLine("D=");
                file.WriteLine("");
                file.WriteLine("[Symmetric]");
                file.WriteLine("Key=");
                file.WriteLine("IV=");
                file.Close();
                file.Dispose();
                file = null;
            }

            IniFileName iniFile = new IniFileName(path);

            //Generate a public/private key pair.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //Save the public key information to an RSAParameters structure.  
            RSAParameters publicInfo = rsa.ExportParameters(false);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.GenerateIV();
            tdes.GenerateKey();

            AsymmetricCryptoHandler crypto = new AsymmetricCryptoHandler(publicInfo, true);
            byte[] key = crypto.Encrypt(tdes.Key);
            byte[] iv = crypto.Encrypt(tdes.IV);

            iniFile.SetValue("Public", "Modulus", Convert.ToBase64String(publicInfo.Modulus));
            iniFile.SetValue("Public", "Exponent", Convert.ToBase64String(publicInfo.Exponent));

            RSAParameters keyInfo = rsa.ExportParameters(true);
            iniFile.SetValue("Private", "Modulus", Convert.ToBase64String(keyInfo.Modulus));
            iniFile.SetValue("Private", "Exponent", Convert.ToBase64String(keyInfo.Exponent));
            iniFile.SetValue("Private", "P", Convert.ToBase64String(keyInfo.P));
            iniFile.SetValue("Private", "Q", Convert.ToBase64String(keyInfo.Q));
            iniFile.SetValue("Private", "DP", Convert.ToBase64String(keyInfo.DP));
            iniFile.SetValue("Private", "DQ", Convert.ToBase64String(keyInfo.DQ));
            iniFile.SetValue("Private", "Inverse", Convert.ToBase64String(keyInfo.InverseQ));
            iniFile.SetValue("Private", "D", Convert.ToBase64String(keyInfo.D));
            iniFile.SetValue("Symmetric", "Key", Convert.ToBase64String(key));
            iniFile.SetValue("Symmetric", "IV", Convert.ToBase64String(iv));

            StreamWriter csFile = new StreamWriter(Environment.CurrentDirectory + @"\csharp.cs", false);
            csFile.WriteLine($"private byte[] _MODULUS = {GenerateArrayString(keyInfo.Modulus)};");
            csFile.WriteLine($"private byte[] _EXPONENT = {GenerateArrayString(keyInfo.Exponent)};");
            csFile.WriteLine($"private byte[] _P = {GenerateArrayString(keyInfo.P)};");
            csFile.WriteLine($"private byte[] _Q = {GenerateArrayString(keyInfo.Q)};");
            csFile.WriteLine($"private byte[] _DP = {GenerateArrayString(keyInfo.DP)};");
            csFile.WriteLine($"private byte[] _DQ = {GenerateArrayString(keyInfo.DQ)};");
            csFile.WriteLine($"private byte[] _INVERSE = {GenerateArrayString(keyInfo.InverseQ)};");
            csFile.WriteLine($"private byte[] _D = {GenerateArrayString(keyInfo.D)};");

            csFile.WriteLine($"");
            csFile.WriteLine($"private byte[] _SYM_KEY = {GenerateArrayString(key)};");
            csFile.WriteLine($"private byte[] _SYM_IV = {GenerateArrayString(iv)};");
            csFile.Close();
            csFile.Dispose();
        }

        private string GenerateArrayString(byte[] array)
        {
            string ret = "{";

            foreach (var val in array)
                ret += $" 0x{val:X2},";

            ret += "}";
            return ret;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = @".dat encrypted file|*.dat",
                Multiselect = false
            };

            if (open.ShowDialog() == DialogResult.OK
                && open.FileName != string.Empty)
            {
                m_szFilename = open.FileName;
                OpenFile();
            }
        }

        private void OpenFile()
        {
            byte[] file = File.ReadAllBytes(m_szFilename);

            SymmetricCryptoHandler symmetricCryptoHandler = new SymmetricCryptoHandler(m_private);
            byte[] decrypt = symmetricCryptoHandler.Decrypt(file);

            output.Text = input.Text = Encoding.ASCII.GetString(decrypt, 0, decrypt.Length);

            input_TextChanged(null, null);
        }
    }
}