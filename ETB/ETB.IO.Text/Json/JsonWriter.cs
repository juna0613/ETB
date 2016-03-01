using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace ETB.IO.Text.Json
{
    public class JsonWriter<T> : IWriter<T>
    {
        private readonly TextWriter _writer;
        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Save(IEnumerable<T> data)
        {
            var str = JsonConvert.SerializeObject(data);
            _writer.Write(str);
        }
    }
}
