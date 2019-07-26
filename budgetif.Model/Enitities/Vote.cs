using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model.Enitities
{
    public class Vote
    {
        public int Id { get; set; }

        public int PollId { get; set; }

        public virtual Poll Poll { get; set; }

        public int DeputatId { get; set; }

        public virtual Deputat Deputat { get; set; }

        public int FractionId { get; set; }

        public virtual Fraction Fraction { get; set; }


        /// <summary>
        /// 0 - No
        /// 1 - Yes
        /// 2 - Abstain
        /// 3 - Not Voted
        /// 4 - Absent
        /// </summary>
        public int Answer { get; set; }

    }

}
