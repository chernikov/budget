using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.GoogleSpreadsheet
{
    public class Spreadsheet
    {
        private SheetsService service { get; set; }

        private string id { get; set; }

        public Spreadsheet(SheetsService service, string id)
        {
            this.service = service;
            this.id = id;
        }


        public void Append(string sheet, IList<IList<object>> values)
        {
            var rangeObj = new ValueRange();
            rangeObj.Values = values;

            var range = sheet;

            var height = values.Count();
            if (height > 0)
            {
                var weight = values[0].Count;
                var columnMax = GetExcelColumnName(weight);
                range += "!A1:" + columnMax + height;
                var update = service.Spreadsheets.Values.Append(rangeObj, this.id, range);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
                update.Execute();
            }
        }


        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
