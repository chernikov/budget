using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class Authority
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }

        public virtual ICollection<PollList> PollLists { get; set; }
    }
}
