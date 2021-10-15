using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventureDay.PortalApi.Helpers
{
    public class XlsxHelper 
    {

        public static string GetSheetId(string sheetName, SpreadsheetDocument doc)
        {
            var sheets = doc.WorkbookPart.Workbook.Sheets;
            for (int s = 0; s < sheets.Count(); s++)
            {
                var sheet = sheets.ElementAt(s) as Sheet;
                if (sheet.Name.Value.ToUpper().Contains(sheetName.ToUpper()))
                {
                    return sheet.Id.Value;
                }
            }

            return string.Empty;
        }

        public static string GetValue(Cell cell, SharedStringTable table)
        {
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                int ssid = int.Parse(cell.CellValue.Text);
                return table.ChildElements[ssid].InnerText;
            }

            if (cell.CellValue != null)
            {
                return cell.CellValue.Text;
            }

            return string.Empty;
        }

        public static int GetColumnIndex(string columnName, Worksheet sheet, SharedStringTable table)
        {
            var rows = sheet.Descendants<Row>();
            var firstRow = rows.First();
            var cellsInFirstRow = firstRow.Descendants<Cell>();
            for (int c = 0; c < cellsInFirstRow.Count(); c++)
            {
                var cell = cellsInFirstRow.ElementAt(c);
                var value = GetValue(cell, table);
                if (value.ToUpper().Contains(columnName.ToUpper()))
                {
                    // maybe we need to parce cell.CellReference here...
                    return c;
                }
            }

            return -1;
        }

    }
}
