using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class PollSubjectNoProcessor : BaseProcessor
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IBudgetIfContext _db;

        CultureInfo provider = CultureInfo.GetCultureInfo("uk");

        private WebConnector _webConnector;

        private Regex titleRegex = new Regex("<div class=\"head_gol\"><FONT COLOR=Black>(?<name>(.|\\n|\\r)*?)</FONT>");

        private Regex numberRegex = new Regex("\\(№(?<number>.*?)\\)");

        private Regex dateRegex = new Regex("</FONT><br>(?<date>(\\d|.|\\s)*?)\n");

        private Regex totalVoteRegex = new Regex("За:(?<yes>(\\d)*?)  Проти:(?<no>(\\d)*?)  Утрималися:(?<abstain>(\\d)*?)  Не голосували:(?<notvoted>(\\d)*?)  ");

        private Regex sessionRegex = new Regex("<tr><td align=\"center\" class=\"f2\">((.|\\r|\\n)*?)(?<session>\\d*?) сесія(.*?)<br>");

        private Regex tableRegex = new Regex("<Table border=1px WIDTH=\"100%\"cellspacing=0 cellpadding=3 align=center>(?<name>(.|\\r|\\n)*?)</TABLE>");

        private Regex vrNumberRegex = new Regex("<b>РЕЗУЛЬТАТИ ПОІМЕННОГО ГОЛОСУВАННЯ</b><br>\n№ (?<number>(\\d*?)) (.*)<br>");

        private Regex rowRegex = new Regex("<tr>((.|\r|\n)*?)</tr>");

        private Regex fractionRegex = new Regex("<td colspan=4><center><b>(?<fraction>(.|\r|\n)*?)</b>");

        private Regex voteRegex = new Regex("<td(.*?)>(?<name>(.|\r|\n)*?)</td>(\r|\n)<td(.*?)>(?<vote>(.|\r|\n)*?)</td>");

        public PollSubjectNoProcessor(IBudgetIfContext db)
        {
            _db = db;
            _webConnector = new WebConnector();
        }

        public override void Run(object input = null)
        {
            logger.Debug("=====START======");
            var lastNumber = 0;
            var currentNumber = lastNumber;
            var list = _db.Polls.Where(p => p.SubjectNo != null && p.SubjectNo.Length < 4).ToList();
            foreach (var item in list)
            {
                GetPoll(item.PageId);
            }
            //var number = 15550;
            //GetPoll(number);
        }

        private void GetPoll(int pollNumber)
        {
            var str = _webConnector.Get($"http://w1.c1.rada.gov.ua/pls/radan_gs09/ns_golos?g_id={pollNumber}", null, Encoding.GetEncoding(1251));
            var printStr = _webConnector.Get($"http://w1.c1.rada.gov.ua/pls/radan_gs09/ns_golos_print?g_id={pollNumber}&vid=1", null, Encoding.GetEncoding(1251));

            var title = GetMatch(str, titleRegex, "name");
            if (string.IsNullOrWhiteSpace(title))
            {
                return;
            }
            var pollEntry = _db.Polls.FirstOrDefault(p => p.PageId == pollNumber);
            if (pollEntry != null)
            {
                var number = GetMatch(title, numberRegex, "number");
                pollEntry.SubjectNo = number;
                logger.Debug($"Number: {number}");
                _db.SaveChanges();
            }
        }


        private string GetMatch(string input, Regex regex, string groupName)
        {
            var match = regex.Match(input);
            if (match.Success)
            {
                var result = match.Groups[groupName].Value;
                return result;
            }
            return null;
        }
    }
}
