using System;
using System.Data;
using Core.FileHandling;
using GenericParsing;

namespace Core.Plugins.Utilities.FileHandling.Delimited
{
    public class GenericParsingDelimitedFileHandler : IDelimitedFileHandler
    {
        public DataTable GetFileAsDataTable(string filepath, char columnDelimiter = ',')
        {
            DataTable dataTable;

            using (var parser = new GenericParserAdapter())
            {
                parser.SetDataSource(filepath);

                parser.ColumnDelimiter = columnDelimiter;
                parser.FirstRowHasHeader = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = Int32.MaxValue;
                parser.TextQualifier = '\"';

                dataTable = parser.GetDataTable();
            }

            return dataTable;
        }
    }
}
