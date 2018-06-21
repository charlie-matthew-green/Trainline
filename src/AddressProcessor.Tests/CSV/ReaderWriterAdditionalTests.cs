using AddressProcessing.CSV;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class ReaderWriterAdditionalTests
    {
        [TestCase(CSVReaderWriterForAnnotation.Mode.Read, @"test_data\contacts.csv")]
        [TestCase(CSVReaderWriterForAnnotation.Mode.Write, @"test_data\emptyFile.csv")]
        public void OpenCalledTwice_NoExceptionThrown(CSVReaderWriterForAnnotation.Mode mode, string inputFilePath)
        {
            var readerWriter = new CsvReaderWriter();
            readerWriter.Open(inputFilePath, mode);
            readerWriter.Open(inputFilePath, mode);
            readerWriter.Close();
        }
    }
}
