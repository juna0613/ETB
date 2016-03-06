using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ETB
{
    public static class TempFileHelper
    {
        public static void CreateTempFolder(Action<string> action, string basePath = null)
        {
            var baseDir = basePath ?? Path.GetTempPath();
            var randomName = Path.GetRandomFileName();
            var dirName = Path.Combine(baseDir, randomName);
            try
            {
                dirName.CreateDirectoryIfNotExist();
                action(dirName);
            }
            finally
            {
                dirName.DeleteDirectoryIfExist(true);
            }
        }
        public static string CreateTempFileName(string basePath = null)
        {
            var tempDir = basePath ?? Path.GetTempPath();
            var name = Path.GetRandomFileName();
            var filePath = Path.Combine(tempDir, name);
            return filePath;
        }
        public static void CreateTempFile(Action<StreamWriter, string> action, string encoding = "SHIFT_JIS")
        {
            var filePath = CreateTempFileName();
            StreamWriter file = new StreamWriter(filePath, true, Encoding.GetEncoding(encoding));
            try
            {
                action(file, filePath);
            }
            finally
            {
                file.Close();
                file.Dispose();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
