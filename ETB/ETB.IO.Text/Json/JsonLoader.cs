using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
namespace ETB.IO.Text.Json
{
    public class JsonLoader<T> : ILoader<T>
    {
        private readonly TextReader _reader;
        public JsonLoader(TextReader reader)
        {
            _reader = reader;
        }
        #region ILoader<T> メンバ

        public IEnumerable<T> Load()
        {
            return JsonConvert.DeserializeObject<List<T>>(_reader.ReadToEnd());
        }

        #endregion
    }
}
