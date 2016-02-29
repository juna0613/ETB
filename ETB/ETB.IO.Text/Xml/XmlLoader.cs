using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
namespace ETB.IO.Text.Xml
{
    public class XmlLoader<T> : ILoader<T>
    {
        private readonly TextReader _reader;
        public XmlLoader(TextReader reader)
        {
            _reader = reader;
        }
        #region ILoader<T> メンバ

        public IEnumerable<T> Load()
        {
            var serializer = new XmlSerializer(typeof(IEnumerable<T>));
            return (IEnumerable<T>)serializer.Deserialize(_reader);
        }

        #endregion
    }
}
