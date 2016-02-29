using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ETB
{
    public static class TempFileHelper
    {
        public static void CreateTempFile(Action<StreamWriter, string> action)
        {
            var tempDir = Path.GetTempPath();
            var name = Path.GetRandomFileName();
            var filePath = Path.Combine(tempDir, name);
            
            StreamWriter file = new StreamWriter(filePath, true, Encoding.GetEncoding("SHIFT_JIS"));
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
