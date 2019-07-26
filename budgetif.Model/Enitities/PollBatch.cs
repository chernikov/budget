using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class PollBatch
    {
        public int Id { get; set; }

        public string SubjectNo { get; set; }

        public string Name { get; set; }

        public bool? IsAccepted { get; set; }

        public string Status { get; set; }

        public virtual ICollection<Poll> Polls { get; set; }

        public virtual ICollection<PollList> PollLists { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
