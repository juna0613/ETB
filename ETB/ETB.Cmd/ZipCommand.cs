using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
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
            try
            {
                Path.GetFullPath(Path.GetDirectoryName(_outputFullPath)).CreateDirectoryIfNotExist();
                var existFiles = _inputFullPaths.Where(File.Exists).ToArray();
                var nonExistFiles = _inputFullPaths.Except(existFiles).ToArray();
                using (var zf = new ZipFile(_outputFullPath))
                {
                    zf.BeginUpdate();
                    foreach (var file in existFiles)
                    {
                        zf.Add(file);
                    }
                    zf.CommitUpdate();
                }
                if(nonExistFiles.Count() > 0)
                {
                    stat = new CommandStatus
                    {
                        Status = -1,
                        Error = "Some missing files : {0}".Format2(nonExistFiles.Join(",")),
                        Output = string.Empty
                    };
                }
                else
                {
                    stat = new CommandStatus
                    {
                        Status = 0,
                        Error = string.Empty,
                        Output = "Successfully zipped"
                    };
                }
            }
            catch(Exception e)
            {
                stat = new CommandStatus
                {
                    Error = e.Message,
                    Status = -1,
                    Output = string.Empty
                };
            }
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
