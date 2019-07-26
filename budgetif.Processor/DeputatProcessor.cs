using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Transactions;

namespace budgetif.Processor
{
    public class DeputatProcessor : BaseProcessor
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

        private List<Deputat> deputatCache = new List<Deputat>();

        private List<Fraction> fractionCache = new List<Fraction>();

        public DeputatProcessor(IBudgetIfContext db)
        {
            _db = db;
            deputatCache = _db.Deputats.ToList();
            fractionCache = _db.Fractions.ToList();
            _webConnector = new WebConnector();
        }

        public override void Run(object input = null)
        {
            logger.Debug("=====START======");
            var lastNumber = _db.Polls.OrderByDescending(p => p.PageId).FirstOrDefault()?.PageId ?? 0;
            var currentNumber = lastNumber;
            while (currentNumber < 20000)
            {
                try
                {
                    var printStr = _webConnector.Get($"http://w1.c1.rada.gov.ua/pls/radan_gs09/ns_golos_print?g_id={currentNumber}&vid=1", null, Encoding.GetEncoding(1251));
                    try
                    {
                        logger.Debug(currentNumber);
                        GetPoll(currentNumber);
                    }
                    catch (Exception ex)
                    {
                        logger.Debug(ex.Message);
                    }
                }
                catch
                {
                }
                currentNumber++;
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
            if (pollEntry == null)
            {
                pollEntry = new Poll();
                pollEntry.Subject = title;

                var number = GetMatch(title, numberRegex, "number");
                pollEntry.SubjectNo = number;


                var date = GetMatch(str, dateRegex, "date");
                logger.Debug("Date: " + date);

                var yes = GetMatch(str, totalVoteRegex, "yes");
                var no = GetMatch(str, totalVoteRegex, "no");
                var abstain = GetMatch(str, totalVoteRegex, "abstain");
                var notvoted = GetMatch(str, totalVoteRegex, "notvoted");

                pollEntry.VoteDate = DateTime.Parse(date, provider);
                pollEntry.Yes = Convert.ToInt32(yes);
                pollEntry.No = Convert.ToInt32(no);
                pollEntry.Abstain = Convert.ToInt32(abstain);
                pollEntry.NotVoted = Convert.ToInt32(notvoted);
                pollEntry.IsAccepted = str.Contains("<FONT COLOR=Green>Рішення прийнято</FONT>");

                var session = GetMatch(printStr, sessionRegex, "session");
                pollEntry.SessionNo = Convert.ToInt32(session);
                var vrNumber = GetMatch(printStr, vrNumberRegex, "number");
                pollEntry.VoteInVRId = Convert.ToInt32(vrNumber);

                pollEntry.PageId = pollNumber;
                _db.Polls.Add(pollEntry);
                _db.SaveChanges();
            }

            var table = GetMatch(printStr, tableRegex, "name");

            var rowMatches = rowRegex.Matches(table);

            Fraction currentFractionEntry = null;
            foreach (Match match in rowMatches)
            {
                var value = match.Groups[0].Value;
                if (value.Contains("colspan=4"))
                {
                    var fraction = GetMatch(value, fractionRegex, "fraction");
                    if (!string.IsNullOrWhiteSpace(fraction))
                    {
                        currentFractionEntry = fractionCache.FirstOrDefault(p => p.Name == fraction);
                        if (currentFractionEntry == null)
                        {
                            currentFractionEntry = new Fraction()
                            {
                                Name = fraction
                            };
                            _db.Fractions.Add(currentFractionEntry);
                            _db.SaveChanges();
                            fractionCache.Add(currentFractionEntry);
                        }
                    }
                }
            }
            logger.Debug("Add Votes");
            var voteList = new List<Vote>();
            foreach (Match match in rowMatches)
            {
                var value = match.Groups[0].Value;
                if (!value.Contains("colspan=4"))
                {
                    var voteMatches = voteRegex.Matches(value);
                    foreach (Match voteMatch in voteMatches)
                    {
                        var name = voteMatch.Groups["name"].Value;
                        var vote = voteMatch.Groups["vote"].Value.Trim();

                        var deputatEntry = deputatCache.FirstOrDefault(p => p.NameWithInitials == name);
                        if (deputatEntry == null)
                        {
                            deputatEntry = new Deputat()
                            {
                                NameWithInitials = name
                            };
                            _db.Deputats.Add(deputatEntry);
                            _db.SaveChanges();
                            deputatCache.Add(deputatEntry);
                        }
                        var answer = 0;
                        switch (vote)
                        {
                            case "Проти":
                                answer = 0;
                                break;
                            case "За":
                                answer = 1;
                                break;
                            case "Утримався":
                            case "Утрималась":
                                answer = 2;
                                break;
                            case "Не голосував":
                            case "Не голосувала":
                                answer = 3;
                                break;
                            case "Відсутня":
                            case "Відсутній":
                                answer = 4;
                                break;
                        }
                        var voteEntry = new Vote()
                        {
                            DeputatId = deputatEntry.Id,
                            FractionId = currentFractionEntry.Id,
                            PollId = pollEntry.Id,
                            Answer = answer
                        };
                        voteList.Add(voteEntry);
                    }
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                BudgetIfContext context = null;
                try
                {
                    context = new BudgetIfContext();
                    context.Configuration.AutoDetectChangesEnabled = false;
                    int count = 0;
                    foreach (var entityToInsert in voteList)
                    {
                        ++count;
                        context = AddToContext(context, entityToInsert, count, 100, true);
                    }
                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

                scope.Complete();
            }
            logger.Debug("Votes added");
        }


        private BudgetIfContext AddToContext(BudgetIfContext context, Vote entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<Vote>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new BudgetIfContext();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
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
