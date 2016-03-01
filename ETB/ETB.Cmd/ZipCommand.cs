using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ETB.Cmd
{
    public class ZipCommand : ICommand
    {
        private readonly string _outputFullPath;
        private readonly string[] _inputFullPaths;
        public ZipCommand(string outputPath, params string[] inputPaths)
        {
            _outputFullPath = Path.GetFullPath(outputPath);
            _inputFullPaths = inputPaths.Select(x => Path.GetFullPath(x)).ToArray();
        }
        #region ICommand メンバ

        public CommandStatus DoCommand()
        {
            CommandStatus stat = null;
            TempFileHelper.CreateTempFile((file, path) =>
            {
                file.WriteLine(_inputFullPaths.Join(Environment.NewLine));
                file.Close();
                var commandStr = @"7z a -tzip {0} @""{1}""".Format2(_outputFullPath, path);
                stat = (new DosCommand(commandStr)).DoCommand();
            });
            return stat;
        }

        #endregion
    }

    public class UnzipCommand : ICommand
    {
        private readonly string _targetZip, _outDir;

        public UnzipCommand(string targetZipPath, string outDirName)
        {
            _targetZip = System.IO.Path.GetFullPath(targetZipPath);
            _outDir = System.IO.Path.GetFullPath(outDirName);
        }
        #region ICommand メンバ

        public CommandStatus DoCommand()
        {
            var stat = (new DosCommand(@"7z x ""{0}"" -o""{1}""".Format2(_targetZip, _outDir)).DoCommand());
            return stat;
        }

        #endregion
    }

}
