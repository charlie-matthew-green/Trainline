using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /// <summary>
    /// Opens a stream to read a csv file a line at a time
    /// </summary>
    public class CsvReader : IDisposable
    {
        private readonly char separator;
        private StreamReader streamReader;
        private bool isReading;
        private bool disposed;

        public CsvReader(char separator)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Inistialises the reader to read from the file at filePath
        /// </summary>
        /// <param name="filePath">The path to the file to be read</param>
        public void Open(string filePath)
        {
            if (!isReading)
            {
                streamReader = new StreamReader(filePath);
                isReading = true;
            }
            else
            {
                Console.WriteLine($"Error - Attempt made to open read stream for '{filePath}' when read stream is already open");
            }
        }

        /// <summary>
        /// Read the next line from the stream and assign the values of the first two columns to column1 and column2 respectively.
        /// If only one column is found then the second column will be set to an empty string.
        /// If there are no more lines method will return false, and column1 and column2 will be null.
        /// </summary>
        /// <param name="column1">Set to the value of the first column</param>
        /// <param name="column2">Set to the value of the second column (or empty string if no column was found)</param>
        /// <returns>Returns true if there was another line to read, false otherwise</returns>
        public bool Read(out string column1, out string column2)
        {
            if (streamReader == null)
            {
                throw new Exception("Attempt made to read before stream reader was initialised");
            }

            var line = streamReader.ReadLine();
            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            var columns = line.Split(separator);
            column1 = columns[0];

            if (columns.Length < 2)
            {
                Console.Write($"Error - Expected at least two columns but only one was found");
                column2 = "";
            }
            else
            {
                column2 = columns[1];
            }

            return true;
        }

        /// <summary>
        /// Not sure what the point of this method is so left here so that the behaviour of the program isn't changed
        /// (even if that behaviour is to throw an exception in some situations)
        /// </summary>
        public bool Read(string column1, string column2)
        {
            var line = streamReader.ReadLine();
            var columns = line.Split(separator);

            column1 = columns[0];
            column2 = columns[1];

            return true;
        }

        /// <summary>
        /// Dispose of the stream
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                streamReader?.Dispose();
                disposed = true;
            }
        }
    }
}
