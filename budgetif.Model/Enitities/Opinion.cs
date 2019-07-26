using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class Opinion
    {
        public int Id { get; set; }

        public int AuthorityId { get; set; }

        public virtual Authority Authority { get; set; }

        public int PollBatchId { get; set; }

        public virtual PollBatch PollBatch { get; set; }

        public int IsSupport { get; set; }

        public int Severity { get; set; }

        public string Description { get; set; }
    }
}
