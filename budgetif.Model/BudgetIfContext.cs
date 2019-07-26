using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgetif.Model
{
    public class BudgetIfContext : DbContext, IBudgetIfContext
    {
        public IDbSet<Authority> Authorities { get; set; }

        public IDbSet<ChatUser> ChatUsers { get; set; }

        public IDbSet<Deputat> Deputats { get; set; }

        public IDbSet<Fraction> Fractions { get; set; }

        public IDbSet<Opinion> Opinions { get; set; }

        public IDbSet<Payer> Payers { get; set; }

        public IDbSet<PollBatch> PollBatches { get; set; }

        public IDbSet<PollList> PollLists { get; set; }

        public IDbSet<Poll> Polls { get; set; }

        public IDbSet<Receipt> Receipts { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<Transaction> Transactions { get; set; }

        public IDbSet<Vote> Votes { get; set; }

        public BudgetIfContext() : base("DbConnection")
        {

        }


        //public BudgetIfContext(string connectionString) : base(connectionString)
        //{

        //}
    }
}
