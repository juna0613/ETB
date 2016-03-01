using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ETB.IO.Text.Csv
{
    public class CSVLoader<T> : ILoader<T>
    {
        private readonly TextReader _reader;
        private readonly bool _hasHeader;
        public CSVLoader(TextReader reader, bool hasHeader = true)
        {
            _reader = reader;
            _hasHeader = hasHeader;
        }
        #region ILoader<T> メンバ

        public IEnumerable<T> Load()
        {
            var data = new List<T>();
            using (var reader = new CsvHelper.CsvReader(_reader, new CsvHelper.Configuration.CsvConfiguration { HasHeaderRecord = _hasHeader }))
            {
                while(reader.Read())
                {
                    data.Add(reader.GetRecord<T>());
                }
            }
            return data;
        }

        #endregion
    }
}
