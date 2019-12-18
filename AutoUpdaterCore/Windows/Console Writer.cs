#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - Console Writer.cs
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
using System.Text;

namespace AutoUpdaterCore.Windows
{
    public class ConsoleWriterEventArgs : EventArgs
    {
        public string Value { get; private set; }

        public ConsoleWriterEventArgs(string value)
        {
            Value = value;
        }
    }

    public class ConsoleWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string value)
        {
            WriteEvent?.Invoke(this, new ConsoleWriterEventArgs(value));
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            WriteLineEvent?.Invoke(this, new ConsoleWriterEventArgs(value));
            base.WriteLine(value);
        }

        public event EventHandler<ConsoleWriterEventArgs> WriteEvent;
        public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;
    }
}