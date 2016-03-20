using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ETB.IO.Text.Json
{
    public class JsonDirWriter<T> : IWriter<T>
    {
        private readonly string _dirPath;
        private readonly Func<T, string> _nameGen;
        private readonly Action<Exception> _errorHandle;
        public JsonDirWriter(string dirPath, Func<T, string> fileNameCreator, Action<Exception> errorHandle = null)
        {
            _dirPath = dirPath;
            _nameGen = fileNameCreator;
            _errorHandle = errorHandle;
        }
        public void Save(IEnumerable<T> data)
        {
            _dirPath.CreateDirectoryIfNotExist();
            foreach(var d in data)
            {
                var path = new[] { _dirPath, _nameGen(d) }.PathJoin();
                using (var writer = new System.IO.StreamWriter(path, false))
                {
                    try
                    {
                        writer.Write(JsonConvert.SerializeObject(d));
                    }
                    catch(Exception e)
                    {
                        if (_errorHandle != null) _errorHandle(e);
                    }
                }
            }
        }
    }
}
