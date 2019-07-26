namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandPolls : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PollListPolls", "PollList_Id", "dbo.PollLists");
            DropForeignKey("dbo.PollListPolls", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.Opinions", "PollId", "dbo.Polls");
            DropIndex("dbo.Opinions", new[] { "PollId" });
            DropIndex("dbo.PollListPolls", new[] { "PollList_Id" });
            DropIndex("dbo.PollListPolls", new[] { "Poll_Id" });
            RenameColumn(table: "dbo.Opinions", name: "PollId", newName: "Poll_Id");
            CreateTable(
                "dbo.PollBatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectNo = c.String(),
                        Name = c.String(),
                        IsAccepted = c.Boolean(),
                        Status = c.String(),
                        PollList_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PollLists", t => t.PollList_Id)
                .Index(t => t.PollList_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PollBatch_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PollBatches", t => t.PollBatch_Id)
                .Index(t => t.PollBatch_Id);
            
            AddColumn("dbo.Opinions", "PollBatchId", c => c.Int(nullable: false));
            AddColumn("dbo.Polls", "PollBatch_Id", c => c.Int());
            AddColumn("dbo.PollLists", "Poll_Id", c => c.Int());
            AlterColumn("dbo.Opinions", "Poll_Id", c => c.Int());
            CreateIndex("dbo.Opinions", "PollBatchId");
            CreateIndex("dbo.Opinions", "Poll_Id");
            CreateIndex("dbo.Polls", "PollBatch_Id");
            CreateIndex("dbo.PollLists", "Poll_Id");
            AddForeignKey("dbo.PollLists", "Poll_Id", "dbo.Polls", "Id");
            AddForeignKey("dbo.Polls", "PollBatch_Id", "dbo.PollBatches", "Id");
            AddForeignKey("dbo.Opinions", "PollBatchId", "dbo.PollBatches", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Opinions", "Poll_Id", "dbo.Polls", "Id");
            DropTable("dbo.PollListPolls");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PollListPolls",
                c => new
                    {
                        PollList_Id = c.Int(nullable: false),
                        Poll_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PollList_Id, t.Poll_Id });
            
            DropForeignKey("dbo.Opinions", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.Opinions", "PollBatchId", "dbo.PollBatches");
            DropForeignKey("dbo.Tags", "PollBatch_Id", "dbo.PollBatches");
            DropForeignKey("dbo.Polls", "PollBatch_Id", "dbo.PollBatches");
            DropForeignKey("dbo.PollLists", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.PollBatches", "PollList_Id", "dbo.PollLists");
            DropIndex("dbo.Tags", new[] { "PollBatch_Id" });
            DropIndex("dbo.PollLists", new[] { "Poll_Id" });
            DropIndex("dbo.Polls", new[] { "PollBatch_Id" });
            DropIndex("dbo.PollBatches", new[] { "PollList_Id" });
            DropIndex("dbo.Opinions", new[] { "Poll_Id" });
            DropIndex("dbo.Opinions", new[] { "PollBatchId" });
            AlterColumn("dbo.Opinions", "Poll_Id", c => c.Int(nullable: false));
            DropColumn("dbo.PollLists", "Poll_Id");
            DropColumn("dbo.Polls", "PollBatch_Id");
            DropColumn("dbo.Opinions", "PollBatchId");
            DropTable("dbo.Tags");
            DropTable("dbo.PollBatches");
            RenameColumn(table: "dbo.Opinions", name: "Poll_Id", newName: "PollId");
            CreateIndex("dbo.PollListPolls", "Poll_Id");
            CreateIndex("dbo.PollListPolls", "PollList_Id");
            CreateIndex("dbo.Opinions", "PollId");
            AddForeignKey("dbo.Opinions", "PollId", "dbo.Polls", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PollListPolls", "Poll_Id", "dbo.Polls", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PollListPolls", "PollList_Id", "dbo.PollLists", "Id", cascadeDelete: true);
        }
    }
}
