using Core.FileHandling;
using GenericParsing;
using System;
using System.Data;
using System.IO;

namespace Core.Plugins.FileHandling.Delimited
{
    public class GenericParsingDelimitedFileHandler : IDelimitedFileHandler
    {
        public DataTable GetFileAsDataTable(string filepath, char columnDelimiter = ',')
        {
            using var parser = new GenericParserAdapter();

            parser.SetDataSource(filepath);

            return FileToDataTable(parser, columnDelimiter);
        }

        public DataTable GetFileStreamAsDataTable(Stream stream, char columnDelimiter = ',')
        {
            using var parser = new GenericParserAdapter();
            using StreamReader streamReader = new StreamReader(stream);

            parser.SetDataSource(streamReader);

            return FileToDataTable(parser, columnDelimiter);
        }

        private DataTable FileToDataTable(GenericParserAdapter parser, char columnDelimiter)
        {
            parser.ColumnDelimiter = columnDelimiter;
            parser.FirstRowHasHeader = true;
            parser.MaxBufferSize = 4096;
            parser.MaxRows = Int32.MaxValue;
            parser.TextQualifier = '\"';

            return parser.GetDataTable();
        }
    }
}
