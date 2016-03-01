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
        private readonly Func<IDictionary<string, string>, T> _mapper;
        public CSVLoader(TextReader reader, bool hasHeader, Func<IDictionary<string, string>, T> mapper)
        {
            _reader = reader;
            _mapper = mapper;
            _hasHeader = hasHeader;
        }
        #region ILoader<T> メンバ

        public IEnumerable<T> Load()
        {
            using (var reader = new CsvHelper.CsvReader(_reader, new CsvHelper.Configuration.CsvConfiguration { HasHeaderRecord = _hasHeader }))
            {
                return reader.GetRecords<T>();
            }
        }

        #endregion
    }
}
