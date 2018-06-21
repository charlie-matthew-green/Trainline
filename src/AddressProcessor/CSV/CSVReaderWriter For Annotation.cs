using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        1) List three to five key concerns with this implementation that you would discuss with the junior developer. 

        Please leave the rest of this file as it is so we can discuss your concerns during the next stage of the interview process.
        
    1) The Open method can initialise either the StreamReader or the StreamWriter, but the Close method closes both of them, meaning
     * that the uses of StreamReader and StreamWriter are unnecessarily tied together. The reading and writing of streams should be
     * separated out into separate classes (single responsibility, separation of concerns). This ensures reading and writing can happen
     * independantly and reduces the chance for error - eg. opening reader when meant to open writer).
    2) Decompiling the Close() and Dispose() methods you can see they both just call Dispose(true) and then GC.SuppressFinalize(this)
     * for StreamWriter, and for StreamReader both methods call Dispose(true), and Dispose then also suppresses Finalization
     * (which presumably is desired). Therefore Dispose can be used instead of Close, and then rather than relying on a separate method
     * being called to ensure that the reader and writer streams are disposed of after use, the using () { ... } pattern should be used
     * in the calling class to ensure there is no way for the objects not to be disposed (eg. if an exception is thrown before Dispose
     * is called, or due to it being forgotten)
    3) What is the point of the 'bool Read(string column1, string column2)' method? It reads the next line and will always return
     * true (since the array returned by Split will always contain at least one element) unless there are no more lines to be read, in
     * which case ReadLine returns null and a null reference exception will be thrown on execution of line.Split(separator). I would
     * assume that the person implementing thought that the string variables passed in to the method would be modified inside the
     * method, but this is not the case since arguments are passed in by value rather than by reference in C#. Ref or out would have to
     * be used for the modifications to column1 and column2 to be visible in the outer scope (as is done in the second Read method).
     * Should this still be here?
    4) In public bool Read(out string column1, out string column2) the check should be if (columns.Length < 2) rather than
     * (columns.Length == 0) to ensure that an out of index exception is not thrown on execution of 'columns[SECOND_COLUMN]'.
    5) It's a bit strange that you can write as many columns as you like but you can only read the first two
    
         */

    public class CSVReaderWriterForAnnotation : ICsvReaderWriter
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
