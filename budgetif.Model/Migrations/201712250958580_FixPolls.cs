namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPolls : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Opinions", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.PollLists", "Poll_Id", "dbo.Polls");
            DropIndex("dbo.Opinions", new[] { "Poll_Id" });
            DropIndex("dbo.PollLists", new[] { "Poll_Id" });
            RenameColumn(table: "dbo.Polls", name: "PollBatch_Id", newName: "PollBatches_Id");
            RenameIndex(table: "dbo.Polls", name: "IX_PollBatch_Id", newName: "IX_PollBatches_Id");
            DropColumn("dbo.Opinions", "Poll_Id");
            DropColumn("dbo.PollLists", "Poll_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PollLists", "Poll_Id", c => c.Int());
            AddColumn("dbo.Opinions", "Poll_Id", c => c.Int());
            RenameIndex(table: "dbo.Polls", name: "IX_PollBatches_Id", newName: "IX_PollBatch_Id");
            RenameColumn(table: "dbo.Polls", name: "PollBatches_Id", newName: "PollBatch_Id");
            CreateIndex("dbo.PollLists", "Poll_Id");
            CreateIndex("dbo.Opinions", "Poll_Id");
            AddForeignKey("dbo.PollLists", "Poll_Id", "dbo.Polls", "Id");
            AddForeignKey("dbo.Opinions", "Poll_Id", "dbo.Polls", "Id");
        }
    }
}
