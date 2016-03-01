using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ETB.IO.Text.Xml
{
    public class XmlWriter<T> : IWriter<T>
    {
        private readonly TextWriter _writer;

        public XmlWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Save(IEnumerable<T> data)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T[]));
            serializer.Serialize(_writer, data.ToArray());
        }
    }
}
