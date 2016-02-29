using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
namespace ETB.IO.Text.Csv
{
    public class CSVDataLoader<T> : IDataLoader
    {
        private readonly bool _hasHeader;
        private readonly TextReader _reader = null;
        public CSVDataLoader(TextReader reader, bool hasHeader)
        {
            _reader = reader;
            _hasHeader = hasHeader;
        }
        #region IDataLoader メンバ

        public DataTable Load()
        {
            var tbl = new DataTable();
            var config = new CsvHelper.Configuration.CsvConfiguration { HasHeaderRecord = _hasHeader };
            using (var reader = new CsvHelper.CsvReader(_reader, config))
            {
                foreach(var d in reader.GetRecords<T>())
                {
                    var row = tbl.NewRow();
                    tbl.Rows.Add(row);
                }
            }
            return tbl;
        }

        #endregion
    }
}
