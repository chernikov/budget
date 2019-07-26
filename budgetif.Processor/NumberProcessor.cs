using budgetif.GoogleSpreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class NumberProcessor : BaseProcessor
    {
        public override void Run(object input = null)
        {
            var spreadsheet = SpreadsheetFactory.GetSpreadsheet("10jO6XNdIrtXm8LuJsjFOO-388o54aNa28CiLiv-31qw");
            var list = (List<string>)input;

            var values = new List<IList<object>>();

            foreach(var item in list)
            {
                values.Add(new List<object>() { item });
            }
            spreadsheet.Append("test", values);
        }
    }
}
