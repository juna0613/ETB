using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ETB.IO.Text.Csv
{
    public class CSVWriter<T> : IWriter<T>
    {
        private readonly TextWriter _writer;
        private readonly Func<T, IDictionary<string, string>> _mapper;
        private readonly char _delimitor;
        private readonly bool _hasHeader;
        public CSVWriter(TextWriter writer, Func<T, IDictionary<string, string>> mapper)
            : this(writer, mapper, ',', true)
        {
        }
        public CSVWriter(TextWriter writer, Func<T, IDictionary<string, string>> mapper, char delimitor, bool hasHeader)
        {
            _writer = writer;
            _mapper = mapper;
            _delimitor = delimitor;
            _hasHeader = hasHeader;
        }
        #region IWriter<T> メンバ

        public void Save(IEnumerable<T> data)
        {
            var config = new CsvHelper.Configuration.CsvConfiguration
            {
                Delimiter = _delimitor.ToString(),
                HasHeaderRecord = _hasHeader,
            };
            using (var writer = new CsvHelper.CsvWriter(_writer, config))
            {
                writer.WriteRecords(data);
            }
        }

        #endregion
    }
}
