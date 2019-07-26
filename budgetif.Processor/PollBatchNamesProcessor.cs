using budgetif.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class PollBatchNamesProcessor : BaseProcessor
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IBudgetIfContext _db;

        public PollBatchNamesProcessor(IBudgetIfContext db)
        {
            _db = db;
        }

        public override void Run(object input = null)
        {
            var list = _db.PollBatches.Include(p => p.Polls).Where(p => p.Polls.Count > 10).OrderByDescending(p => p.Polls.Count).ToList();

            foreach(var item in list)
            {
                logger.Debug($"{item.Name} {item.SubjectNo}");
                var polls = _db.Polls.Where(p => p.PollBatchId == item.Id).OrderByDescending(p => p.VoteDate).ToList();
                var first = polls.FirstOrDefault(p => p.Subject.Contains(" - за основу"));
                var second = polls.FirstOrDefault(p => p.Subject.Contains(" - у другому читанні та в цілому"));
                if (second != null)
                {
                    item.Name = second.Subject.Replace(" - у другому читанні та в цілому", "");
                    item.IsAccepted = second.IsAccepted;
                    _db.SaveChanges();

                } else if (first != null)
                {
                    item.Name = first.Subject.Replace(" - за основу", "");
                    item.IsAccepted = first.IsAccepted;
                    _db.SaveChanges();
                }
                
            }
        }
    }
}
