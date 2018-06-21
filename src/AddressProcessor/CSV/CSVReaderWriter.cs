using System;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */


    public interface ICsvReaderWriter
    {
        void Open(string filePath, CSVReaderWriterForAnnotation.Mode mode);
        void Write(params string[] columns);
        bool Read(out string column1, out string column2);
        void Close();

        [Obsolete("Here for backwards compatability only, do not use")]
        bool Read(string column1, string column2);
    }

    /// <summary>
    /// Exists for backwards compatability. Recommended to use CsvReader and CsvWriter individually rather than through this class.
    /// </summary>
    public class CsvReaderWriter : ICsvReaderWriter, IDisposable
    {
        private char separator = '\t';
        private readonly CsvReader csvReader;
        private readonly CsvWriter csvWriter;

        public CsvReaderWriter()
        {
            csvReader = new CsvReader(separator);
            csvWriter = new CsvWriter(separator);
        }

        public void Open(string filePath, CSVReaderWriterForAnnotation.Mode mode)
        {
            switch (mode)
            {
                case CSVReaderWriterForAnnotation.Mode.Read:
                {
                    csvReader.Open(filePath);
                    return;
                }
                case CSVReaderWriterForAnnotation.Mode.Write:
                {
                    csvWriter.Open(filePath);
                    return;
                }
                default: throw new Exception($"Unexected mode {mode.ToString()}. Cannot open file at '{filePath}'");
            }
        }

        public void Write(params string[] columns)
        {
            csvWriter.Write(columns);
        }

        public bool Read(string column1, string column2)
        {
            return csvReader.Read(column1, column2);
        }

        public bool Read(out string column1, out string column2)
        {
            return csvReader.Read(out column1, out column2);
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            csvReader?.Dispose();
            csvWriter?.Dispose();
        }
    }
}
