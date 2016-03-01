using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

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
            // this is a workaround to keep white space in string
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(_reader);
            using (XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement))
            {
                var serializer = new XmlSerializer(typeof(T[]));
                return (IEnumerable<T>)serializer.Deserialize(reader);
            }
        }

        #endregion
    }
}
