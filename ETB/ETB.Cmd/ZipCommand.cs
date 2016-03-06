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
            Console.WriteLine("ZipCommand----------------");
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
                var nonExistFiles = new List<string>();
                TempFileHelper.CreateTempFolder(dir =>
                {
                    foreach(var fileOrDir in _inputFullPaths)
                    {
                        if(File.Exists(fileOrDir))
                        {
                            var dest = Path.Combine(dir, Path.GetFileName(fileOrDir));
                            File.Copy(fileOrDir, dest, true);
                        }
                        else if(Directory.Exists(fileOrDir))
                        {
                            fileOrDir.CopyDirectoryTo(dir, true);
                        }
                        else
                        {
                            nonExistFiles.Add(fileOrDir);
                        }
                    }
                    var fastZip = new FastZip();
                    fastZip.CreateEmptyDirectories = true;
                    fastZip.CreateZip(_outputFullPath, dir, true, string.Empty);
                });
                //using (var zf = ZipFile.Create(_outputFullPath))
                //{
                    
                //    zf.BeginUpdate();
                //    foreach (var fileOrDir in _inputFullPaths)
                //    {
                //        var folderName = Path.GetFullPath(Path.GetDirectoryName(fileOrDir));
                //        var trans = new ZipNameTransform(folderName);
                //        if(File.Exists(fileOrDir))
                //        {
                //            var transName = trans.TransformFile(fileOrDir);
                //            zf.Add(fileOrDir, transName);
                //        }
                //        else if(Directory.Exists(fileOrDir))
                //        {
                //            var transName = trans.TransformDirectory(fileOrDir);
                //            zf.AddDirectory(transName);
                //        }
                //        else // not found
                //        {
                //            nonExistFiles.Add(fileOrDir);
                //        }
                //    }
                //    zf.CommitUpdate();
                //}
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
                        Output = "Successfully zipped {0}".Format2(_inputFullPaths.Join(","))
                    };
                }
            }
            catch(Exception e)
            {
                stat = new CommandStatus
                {
                    Error = e.PrettyPrint() ?? string.Empty,
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
