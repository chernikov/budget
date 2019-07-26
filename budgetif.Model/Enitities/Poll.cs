using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class Poll
    {
        public int Id { get; set; }

        public int VoteInVRId { get; set; }

        public int PageId { get; set; }

        public int SessionNo { get; set; }

        public string Subject { get; set; }

        public string SubjectNo { get; set; }

        public DateTime VoteDate { get; set; }

        public int Yes { get; set; }

        public int No { get; set; }

        public int Abstain { get; set; }

        public int NotVoted { get; set; }

        public int Absent { get; set; }

        public bool IsAccepted { get; set; }

        public int? PollBatchId { get; set; }

        public virtual PollBatch PollBatch { get; set; }
    }
}
