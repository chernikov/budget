using budgetif.Model.Enitities;
using System.Data.Entity;

namespace budgetif.Model
{
    public interface IBudgetIfContext
    {

        IDbSet<Authority> Authorities { get; set; }

        IDbSet<ChatUser> ChatUsers { get; set; }

        IDbSet<Deputat> Deputats { get; set; }

        IDbSet<Fraction> Fractions { get; set; }

        IDbSet<Opinion> Opinions { get; set; }

        IDbSet<Payer> Payers { get; set; }

        IDbSet<PollBatch> PollBatches { get; set; }

        IDbSet<PollList> PollLists { get; set; }

        IDbSet<Poll> Polls { get; set; }

        IDbSet<Receipt> Receipts { get; set; }

        IDbSet<Tag> Tags { get; set; }

        IDbSet<Transaction> Transactions { get; set; }

        IDbSet<Vote> Votes { get; set; }

        int SaveChanges();
    }
}