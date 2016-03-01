using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ETB
{
    public static class TempFileHelper
    {
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
