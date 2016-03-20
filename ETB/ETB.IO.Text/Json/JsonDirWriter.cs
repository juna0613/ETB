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
        public JsonDirWriter(string dirPath, Func<T, string> fileNameCreator)
        {
            _dirPath = dirPath;
            _nameGen = fileNameCreator;
        }
        public void Save(IEnumerable<T> data)
        {
            _dirPath.CreateDirectoryIfNotExist();
            var exceptions = new List<Exception>();
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
                        exceptions.Add(e);
                    }
                }
            }
            if(exceptions.Count > 0)
            {
                throw new MultiException(exceptions.ToArray());
            }
        }
    }
}
