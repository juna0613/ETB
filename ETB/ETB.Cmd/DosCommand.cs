using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace ETB.Cmd
{
    public class DosCommand : ICommand
    {
        private readonly string _command;
        public DosCommand(string command)
        {
            _command = command;
        }

        #region ICommand メンバ

        public CommandStatus DoCommand()
        {
            var p = new Process();
            p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = string.Format("/c {0}", _command);
            p.Start();
            p.WaitForExit();
            var res = new CommandStatus
            {
                Output = p.StandardOutput.ReadToEnd(),
                Error = p.StandardError.ReadToEnd(),
                Status = p.ExitCode
            };
            return res;
        }

        #endregion
    }
}
