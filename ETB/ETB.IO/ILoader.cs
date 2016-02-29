using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace ETB.IO
{
    public interface ILoader<T>
    {
        IEnumerable<T> Load();
    }

    public interface IDataLoader
    {
        DataTable Load();
    }
}
