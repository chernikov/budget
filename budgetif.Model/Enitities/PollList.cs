using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class PollList
    {
        public int Id { get; set; }

        public int? AuthorityId { get; set; }

        public virtual Authority Authority { get; set; }

        public int? ChatUserId { get; set; }

        public virtual ChatUser ChatUser { get; set; }

        public virtual ICollection<PollBatch> PollBatches { get; set; }
    }
}
