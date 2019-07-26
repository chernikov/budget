using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Processor
{
    public class PollBatchProcessor : BaseProcessor
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IBudgetIfContext _db;

        private List<PollBatch> _pollBatchesCache;

        public PollBatchProcessor(IBudgetIfContext db)
        {
            _db = db;
            _pollBatchesCache = _db.PollBatches.ToList();
        }
        public override void Run(object input = null)
        {
            var list = _db.Polls.Where(p => p.SubjectNo != null && p.PollBatchId == null).ToList();

            foreach(var item in list)
            {
                var batch = _pollBatchesCache.FirstOrDefault(p => p.SubjectNo == item.SubjectNo);

                if (batch == null)
                {
                    batch = new PollBatch()
                    {
                        Name = "",
                        SubjectNo = item.SubjectNo
                    };
                    logger.Debug($"Add batch {item.SubjectNo}");

                    _db.PollBatches.Add(batch);
                    _db.SaveChanges();
                    _pollBatchesCache.Add(batch);
                }
                item.PollBatchId = batch.Id;
                _db.SaveChanges();
            }
        }
    }
}
