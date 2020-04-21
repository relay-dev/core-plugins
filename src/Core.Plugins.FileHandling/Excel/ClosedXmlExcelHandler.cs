using System;
using System.Data;
using ClosedXML.Excel;
using Core.FileHandling;
using System.IO;
using System.Collections.Generic;

namespace Core.Plugins.Utilities.FileHandling.Excel
{
    public class ClosedXmlExcelHandler : IExcelFileHandler
    {
        public DataTable GetWorksheetAsDataTable(string filePath, int sheetPosition = 1)
        {
            var workbook = new XLWorkbook(filePath);

            IXLWorksheet worksheet = workbook.Worksheet(sheetPosition);

            return ConvertWorksheetToDataTable(worksheet);
        }

        public DataTable GetWorksheetAsDataTable(string filePath, string sheetName)
        {
            var workbook = new XLWorkbook(filePath);

            IXLWorksheet worksheet = workbook.Worksheet(sheetName);

            return ConvertWorksheetToDataTable(worksheet);
        }

        public DataTable GetWorksheetAsDataTable(Stream fileStream, int sheetPosition = 1)
        {
            var workbook = new XLWorkbook(fileStream);

            IXLWorksheet worksheet = workbook.Worksheet(sheetPosition);

            return ConvertWorksheetToDataTable(worksheet);
        }

        public DataTable GetWorksheetAsDataTable(Stream fileStream, string sheetName)
        {
            var workbook = new XLWorkbook(fileStream);

            IXLWorksheet worksheet = workbook.Worksheet(sheetName);

            return ConvertWorksheetToDataTable(worksheet);
        }

        public byte[] CreateWorkbookFromDataTable(DataTable dataTable, string sheetName = "Sheet1")
        {
            var wb = new XLWorkbook();

            wb.Worksheets.Add(dataTable, sheetName);

            byte[] data;

            using (var outerStream = new MemoryStream())
            {
                wb.SaveAs(outerStream);
                outerStream.Position = 0;

                using (var innerStream = new MemoryStream())
                {
                    outerStream.CopyTo(innerStream);
                    data = innerStream.ToArray();
                }
            }

            return data;
        }

        public byte[] CreateWorkbookFromDataTables(List<DataTable> dataTables)
        {
            var wb = new XLWorkbook();

            foreach (DataTable dataTable in dataTables)
            {
                IXLWorksheet worksheet = wb.Worksheets.Add(dataTable, dataTable.TableName);

                if (worksheet.Cell(1, 1).Value.ToString() == "A" && worksheet.Cell(1, 2).Value.ToString() == "B")
                {
                    worksheet.Row(1).Hide();
                }
            }

            byte[] data;

            using (var outerStream = new MemoryStream())
            {
                wb.SaveAs(outerStream);
                outerStream.Position = 0;

                using (var innerStream = new MemoryStream())
                {
                    outerStream.CopyTo(innerStream);
                    data = innerStream.ToArray();
                }
            }

            return data;
        }

        public void SaveWorkbookFromDataTable(DataTable dataTable, string filePath, string sheetName = "Sheet1")
        {
            var xLWorkbook = new XLWorkbook();

            xLWorkbook.Worksheets.Add(dataTable, sheetName);

            xLWorkbook.SaveAs(filePath);
        }

        #region Private

        private DataTable ConvertWorksheetToDataTable(IXLWorksheet worksheet)
        {
            var dataTable = new DataTable();

            List<int> formattedColumnNumbers = new List<int>();

            IXLRange range = worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed());

            int columnCount = range.ColumnCount();

            dataTable.Clear();

            for (int i = 1; i <= columnCount; i++)
            {
                IXLCell cell = worksheet.Cell(1, i);

                Type dataTableColumnType = GetDataType(range, cell.Address.ColumnNumber);

                dataTable.Columns.Add(cell.Value.ToString(), dataTableColumnType);
            }

            int firstHeadRow = 0;

            foreach (var row in range.Rows())
            {
                if (firstHeadRow != 0)
                {
                    var array = new object[columnCount];

                    for (int y = 1; y <= columnCount; y++)
                    {
                        if (!row.Cell(y).HasFormula)
                        {
                            if (row.Cell(y).Value == null || string.IsNullOrEmpty(row.Cell(y).Value.ToString()))
                            {
                                array[y - 1] = null;
                            }
                            else if (formattedColumnNumbers.Contains(row.Cell(y).Address.ColumnNumber))
                            {
                                array[y - 1] = row.Cell(y).GetFormattedString();
                            }
                            else if (row.Cell(y).DataType == XLDataType.DateTime)
                            {
                                DateTime dateTime = row.Cell(y).GetDateTime();

                                array[y - 1] = dateTime.TimeOfDay.Ticks == 0
                                    ? dateTime.ToString("MM/dd/yyyy")
                                    : dateTime.ToString();
                            }
                            else
                            {
                                row.Cell(y).SetDataType(XLDataType.Text);

                                array[y - 1] = row.Cell(y).Value;
                            }
                        }
                    }

                    dataTable.Rows.Add(array);
                }

                firstHeadRow++;
            }

            return dataTable;
        }

        private Type GetDataType(IXLRange range, int columnNumber)
        {
            IXLCell cell = GetFirstNonNullCell(range, columnNumber);

            if (cell != null)
            {
                if (cell.DataType == XLDataType.DateTime)
                    return typeof(DateTime);

                if (cell.DataType == XLDataType.Boolean)
                    return typeof(bool);

                if (cell.DataType == XLDataType.Number)
                    return typeof(int);
            }

            return typeof(string);
        }

        private IXLCell GetFirstNonNullCell(IXLRange range, int columnNumber)
        {
            bool isDataFiled = false;

            foreach (var row in range.Rows())
            {
                if (!isDataFiled)
                {
                    isDataFiled = true;
                    continue;
                }

                object cellValue = row.Cell(columnNumber).Value;

                if (!string.IsNullOrEmpty(cellValue.ToString()))
                {
                    return row.Cell(columnNumber);
                }
            }

            return null;
        }

        #endregion
    }
}
