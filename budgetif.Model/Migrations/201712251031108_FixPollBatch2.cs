namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPollBatch2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PollBatches", "PollList_Id", "dbo.PollLists");
            DropIndex("dbo.PollBatches", new[] { "PollList_Id" });
            CreateTable(
                "dbo.PollListPollBatches",
                c => new
                    {
                        PollList_Id = c.Int(nullable: false),
                        PollBatch_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PollList_Id, t.PollBatch_Id })
                .ForeignKey("dbo.PollLists", t => t.PollList_Id, cascadeDelete: true)
                .ForeignKey("dbo.PollBatches", t => t.PollBatch_Id, cascadeDelete: true)
                .Index(t => t.PollList_Id)
                .Index(t => t.PollBatch_Id);
            
            DropColumn("dbo.PollBatches", "PollList_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PollBatches", "PollList_Id", c => c.Int());
            DropForeignKey("dbo.PollListPollBatches", "PollBatch_Id", "dbo.PollBatches");
            DropForeignKey("dbo.PollListPollBatches", "PollList_Id", "dbo.PollLists");
            DropIndex("dbo.PollListPollBatches", new[] { "PollBatch_Id" });
            DropIndex("dbo.PollListPollBatches", new[] { "PollList_Id" });
            DropTable("dbo.PollListPollBatches");
            CreateIndex("dbo.PollBatches", "PollList_Id");
            AddForeignKey("dbo.PollBatches", "PollList_Id", "dbo.PollLists", "Id");
        }
    }
}
