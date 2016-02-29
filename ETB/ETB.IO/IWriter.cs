using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace ETB.IO
{
    public interface IWriter<T>
    {
        void Save(IEnumerable<T> data);
    }

    public interface IDataWriter
    {
        void Save(DataTable data);
    }
}
