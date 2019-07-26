using budgetif.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class TerrNumberProcessor : BaseProcessor
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IBudgetIfContext _db;

        private Regex numberRegex = new Regex("ОВО №(?<number>.*?)</A>(.|\r|\n)*?<a.*?>(?<deputat>.*?) </A>");

        CultureInfo provider = CultureInfo.GetCultureInfo("uk");

        private WebConnector _webConnector;

        public TerrNumberProcessor(IBudgetIfContext db)
        {
            _db = db;
            _webConnector = new WebConnector();
        }

        public override void Run(object input = null)
        {
            logger.Debug("=====START======");

            //var printStr = _webConnector.Get($"http://www.cvk.gov.ua/pls/vnd2014/wp039?PT001F01=910", null, Encoding.GetEncoding(1251));
            var printStr = _webConnector.Get($"http://www.cvk.gov.ua/pls/vnd2014/wp039?PT001F01=914", null, Encoding.GetEncoding(1251));
            var rowMatches = numberRegex.Matches(printStr);

            foreach (Match rowMatch in rowMatches)
            {
                var number = rowMatch.Groups["number"].Value;
                var deputat = rowMatch.Groups["deputat"].Value;

                var deputatParts = deputat.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var deputatName = $"{deputatParts[0]} {deputatParts[1].ToUpper()[0]}.{deputatParts[2].ToUpper()[0]}.";

                var deputatEntry = _db.Deputats.FirstOrDefault(p => p.NameWithInitials == deputatName);

                if (deputatEntry != null)
                {
                    deputatEntry.TerrNumber = Convert.ToInt32(number);
                    deputatEntry.IsMajor = true;
                    _db.SaveChanges();
                }
            }
        }
    }
}
