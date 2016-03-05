using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace ETB.Cmd
{

    public class AsyncDosCommand
    {
        public event EventHandler Exited;
        private readonly string _command;
        public AsyncDosCommand(string command)
        {
            _command = command;
        }

        public virtual void DoCommand()
        {
            var p = new Process();
            p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = string.Format("/c {0}", _command);
            p.EnableRaisingEvents = true;
            p.Exited += Exited;
            
            p.Start();
        }
    }
}
