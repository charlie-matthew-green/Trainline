using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /// <summary>
    /// Opens a stream to create a csv file from arrays of strings a line at a time.
    /// The stream is written to csv file when the object is disposed.
    /// If the file at the filepath specified does not exist a file will be created, else if it does exist it will be overwritten 
    /// </summary>
    public class CsvWriter
    {
        private readonly char separator;
        private StreamWriter streamWriter;
        private bool isWriting;
        private bool disposed;

        public CsvWriter(char separator)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Initialises the stream to write to
        /// </summary>
        /// <param name="filePath">The path to the file to write to</param>
        public void Open(string filePath)
        {
            if (!isWriting)
            {
                streamWriter = new StreamWriter(filePath, false);
                isWriting = true;
            }
            else
            {
                Console.WriteLine($"Error - Attempt made to open write stream for '{filePath}' when write stream is already open");
            }
        }

        /// <summary>
        /// Creates the next line of the csv file and writes it to the stream
        /// </summary>
        /// <param name="columns">The columns to be written to the next line of the csv file</param>
        public void Write(params string[] columns)
        {
            if (streamWriter == null)
            {
                throw new Exception("Attempt made to write before stream writer was initialised");
            }

            var line = string.Join(separator.ToString(), columns);
            streamWriter.WriteLine(line);
        }

        /// <summary>
        /// Dispose the stream and write to file
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                streamWriter?.Dispose();
                disposed = true;
            }
        }
    }
}
