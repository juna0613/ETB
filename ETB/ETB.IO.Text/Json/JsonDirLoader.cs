using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace ETB.IO.Text.Json
{
    public class JsonDirLoader<T> : ILoader<T>
    {
        private readonly string _dirPath;
        private readonly Action<Exception> _errorHandle;
        public JsonDirLoader(string dirPath, Action<Exception> errorHandle = null)
        {
            _dirPath = dirPath;
            _errorHandle = errorHandle;
        }
        public IEnumerable<T> Load()
        {
            AssertionHelper.DoAssert(helper =>
            {
                helper.Add(Directory.Exists(_dirPath), "{0} not exist".Format2(_dirPath));
            });
            var ret = new List<T>();
            foreach(var path in Directory.GetFiles(_dirPath))
            {
                try
                {
                    var str = File.ReadAllText(path);
                    var data = JsonConvert.DeserializeObject<T>(str);
                    ret.Add(data);
                }
                catch(Exception e)
                {
                    if (_errorHandle != null) _errorHandle(e);
                }
            }
            return ret;
        }
    }
}
