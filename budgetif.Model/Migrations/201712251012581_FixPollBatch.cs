namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPollBatch : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Polls", name: "PollBatches_Id", newName: "PollBatchId");
            RenameIndex(table: "dbo.Polls", name: "IX_PollBatches_Id", newName: "IX_PollBatchId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Polls", name: "IX_PollBatchId", newName: "IX_PollBatches_Id");
            RenameColumn(table: "dbo.Polls", name: "PollBatchId", newName: "PollBatches_Id");
        }
    }
}
