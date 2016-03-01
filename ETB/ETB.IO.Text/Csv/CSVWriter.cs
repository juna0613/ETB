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
        private readonly char _delimitor;
        private readonly bool _hasHeader;
        
        public CSVWriter(TextWriter writer, char delimitor = ',', bool hasHeader = true)
        {
            _writer = writer;
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
