using System;
using System.IO;
using AddressProcessing.CSV;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvReaderWriterTests
    {
        [Test]
        public void WritesColumns()
        {
            // check old implementation
            var csvReaderWriterOld = new CSVReaderWriterForAnnotation();
            AssertWritesColumns(csvReaderWriterOld);

            // check new implementation
            var csvReaderWriterNew = new CsvReaderWriter();
            AssertWritesColumns(csvReaderWriterNew);
        }

        [Test]
        public void ReadsAllLines()
        {
            // check old implementation
            var csvReaderWriterOld = new CSVReaderWriterForAnnotation();
            AssertReadsAllLines(csvReaderWriterOld);

            // check new implementation
            var csvReaderWriterNew = new CsvReaderWriter();
            AssertReadsAllLines(csvReaderWriterNew);
        }

        [Test]
        public void NonUsefulReadMethodKeepsOldBehaviour()
        {
            // check old implementation
            var csvReaderWriterOld = new CSVReaderWriterForAnnotation();
            AssertReturnsTrueOrThrowsException(csvReaderWriterOld);

            // check new implementation
            var csvReaderWriterNew = new CsvReaderWriter();
            AssertReturnsTrueOrThrowsException(csvReaderWriterNew);
        }

        private void AssertWritesColumns(ICsvReaderWriter csvReaderWriter)
        {
            // Arrange
            const string dataFile = @"test_data\emptyFile.csv";
            csvReaderWriter.Open(dataFile, CSVReaderWriterForAnnotation.Mode.Write);

            // Act
            csvReaderWriter.Write("Column 1A", "Column 1B");
            csvReaderWriter.Write("Column 2A", "Column 2B");
            csvReaderWriter.Close();
            var linesAfterWrite = File.ReadAllLines(dataFile);

            // Assert

            // Check only correct number of lines have been written
            Assert.AreEqual(2, linesAfterWrite.Length);

            // Check line added is expected string
            Assert.AreEqual("Column 1A\tColumn 1B", linesAfterWrite[0]);
            Assert.AreEqual("Column 2A\tColumn 2B", linesAfterWrite[1]);
        }

        private void AssertReadsAllLines(ICsvReaderWriter csvReaderWriter)
        {
            // Arrange
            const string dataFile = @"test_data\contacts.csv";
            var allLines = File.ReadAllLines(dataFile);
            csvReaderWriter.Open(dataFile, CSVReaderWriterForAnnotation.Mode.Read);

            // Act and Assert
            var lineNumber = 0;
            while (csvReaderWriter.Read(out var column1, out var column2))
            {
                var nextLine = allLines[lineNumber];
                var columns = nextLine.Split('\t');
                Assert.AreEqual(columns[0], column1);
                Assert.AreEqual(columns[1], column2);
                lineNumber++;
            }
            csvReaderWriter.Close();
            Assert.AreEqual(allLines.Length, lineNumber);
        }

        private void AssertReturnsTrueOrThrowsException(ICsvReaderWriter csvReaderWriter)
        {
            // Arrange
            const string dataFile = @"test_data\contacts.csv";
            var lineCount = File.ReadAllLines(dataFile).Length;
            csvReaderWriter.Open(dataFile, CSVReaderWriterForAnnotation.Mode.Read);

            // Act and Assert
            for (var i = 0; i < lineCount; i++)
            {
                Assert.AreEqual(true, csvReaderWriter.Read("", ""));
            }
            Assert.Throws<NullReferenceException>(() => csvReaderWriter.Read("", ""));
        }
    }
}
