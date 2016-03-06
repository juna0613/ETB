using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.Cmd
{
    public enum FtpMode
    {
        ASCII,
        BINARY
    }
    
    public class FtpPutCommand : ICommand
    {
        private readonly string _ftpSite, _user, _password;
        private readonly string[] _files;
        private readonly string _command;
        private readonly FtpMode _mode;
        public FtpPutCommand(string ftpSite, string user, string password, FtpMode mode, params string[] files)
        {
            _ftpSite = ftpSite;
            _user = user;
            _password = password;
            _files = files;
            _mode = mode;
            _command = new[] { string.Format("open {0}", _ftpSite), _user, _password, "prompt", _mode.ToString().ToLower() }
                .Union(_files.Select(x => string.Format("put \"{0}\"", x))).Union(new[] { "bye" }).Join(Environment.NewLine);
        }
        #region ICommand メンバ

        public CommandStatus DoCommand()
        {
            CommandStatus dosstat = null;
            TempFileHelper.CreateTempFile((file, path) =>
            {
                file.Write(_command);
                file.Close();
                var ftpCommand = string.Format("ftp -v -s:\"{0}\"", path);
                dosstat = (new DosCommand(ftpCommand)).DoCommand();
            });

            var errorMsg = "Ftp Command has error(s) in some reasons: command {0}{1}".Format2(Environment.NewLine, _command.Replace(_password, "********"));
            if (dosstat == null)
            {
                return new CommandStatus
                {
                    Status = -1,
                    Error = errorMsg,
                    Output = string.Empty
                };
            }
            var successNum = dosstat.Output.CountMatches(@"226 Transfer complete");
            var status =  successNum == _files.Length ? 0 : -1;
            var ret = new CommandStatus
            {
                Output = dosstat.Output,
                Status = status,
                Error = status == 0 ? "" : "Success {0} out of {1}. Error Message: {2}".Format2(successNum, _files.Length, errorMsg)
            };
            return ret;
        }

        #endregion

        
    }
}
