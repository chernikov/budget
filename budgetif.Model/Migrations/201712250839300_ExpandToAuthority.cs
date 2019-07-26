namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandToAuthority : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Opinions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorityId = c.Int(nullable: false),
                        PollId = c.Int(nullable: false),
                        IsSupport = c.Int(nullable: false),
                        Severity = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authorities", t => t.AuthorityId, cascadeDelete: true)
                .ForeignKey("dbo.Polls", t => t.PollId, cascadeDelete: true)
                .Index(t => t.AuthorityId)
                .Index(t => t.PollId);
            
            CreateTable(
                "dbo.PollLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorityId = c.Int(),
                        ChatUserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authorities", t => t.AuthorityId)
                .ForeignKey("dbo.ChatUsers", t => t.ChatUserId)
                .Index(t => t.AuthorityId)
                .Index(t => t.ChatUserId);
            
            CreateTable(
                "dbo.PollListPolls",
                c => new
                    {
                        PollList_Id = c.Int(nullable: false),
                        Poll_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PollList_Id, t.Poll_Id })
                .ForeignKey("dbo.PollLists", t => t.PollList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Polls", t => t.Poll_Id, cascadeDelete: true)
                .Index(t => t.PollList_Id)
                .Index(t => t.Poll_Id);
            
            AddColumn("dbo.Deputats", "AvatarPath", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PollListPolls", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.PollListPolls", "PollList_Id", "dbo.PollLists");
            DropForeignKey("dbo.PollLists", "ChatUserId", "dbo.ChatUsers");
            DropForeignKey("dbo.PollLists", "AuthorityId", "dbo.Authorities");
            DropForeignKey("dbo.Opinions", "PollId", "dbo.Polls");
            DropForeignKey("dbo.Opinions", "AuthorityId", "dbo.Authorities");
            DropIndex("dbo.PollListPolls", new[] { "Poll_Id" });
            DropIndex("dbo.PollListPolls", new[] { "PollList_Id" });
            DropIndex("dbo.PollLists", new[] { "ChatUserId" });
            DropIndex("dbo.PollLists", new[] { "AuthorityId" });
            DropIndex("dbo.Opinions", new[] { "PollId" });
            DropIndex("dbo.Opinions", new[] { "AuthorityId" });
            DropColumn("dbo.Deputats", "AvatarPath");
            DropTable("dbo.PollListPolls");
            DropTable("dbo.PollLists");
            DropTable("dbo.Opinions");
            DropTable("dbo.Authorities");
        }
    }
}
