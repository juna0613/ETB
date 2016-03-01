using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB.IO.Text.Json
{
    public class JsonWriter<T> : IWriter<T>
    {
        public void Save(IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }
    }
}
