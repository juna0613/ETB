using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ETB
{
    public static class FileExtensions
    {
        public static void DeleteIfExist(this string filePath)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void SafeCreateFile(this string filePath)
        {
            var dirName = Path.GetFullPath(Path.GetDirectoryName(filePath));
            dirName.CreateDirectoryIfNotExist();
            File.Create(filePath);
        }

        public static void DeleteDirectoryIfExist(this string directoryPath, bool recursive = true)
        {
            if(Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, recursive);
            }
        }

        public static void CreateDirectoryIfNotExist(this string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        
    }
}
