namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VerhovnaRada : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deputats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TerrNumber = c.Int(),
                        IsMajor = c.Boolean(nullable: false),
                        NameWithInitials = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Patronymic = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fractions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Polls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VoteInVRId = c.Int(nullable: false),
                        PageId = c.Int(nullable: false),
                        SessionNo = c.Int(nullable: false),
                        Subject = c.String(),
                        SubjectNo = c.Int(nullable: false),
                        VoteDate = c.DateTime(nullable: false),
                        Yes = c.Int(nullable: false),
                        No = c.Int(nullable: false),
                        Abstain = c.Int(nullable: false),
                        NotVoted = c.Int(nullable: false),
                        Absent = c.Int(nullable: false),
                        IsAccepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PollId = c.Int(nullable: false),
                        DeputatId = c.Int(nullable: false),
                        FractionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deputats", t => t.DeputatId, cascadeDelete: true)
                .ForeignKey("dbo.Fractions", t => t.FractionId, cascadeDelete: true)
                .ForeignKey("dbo.Polls", t => t.PollId, cascadeDelete: true)
                .Index(t => t.PollId)
                .Index(t => t.DeputatId)
                .Index(t => t.FractionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "PollId", "dbo.Polls");
            DropForeignKey("dbo.Votes", "FractionId", "dbo.Fractions");
            DropForeignKey("dbo.Votes", "DeputatId", "dbo.Deputats");
            DropIndex("dbo.Votes", new[] { "FractionId" });
            DropIndex("dbo.Votes", new[] { "DeputatId" });
            DropIndex("dbo.Votes", new[] { "PollId" });
            DropTable("dbo.Votes");
            DropTable("dbo.Polls");
            DropTable("dbo.Fractions");
            DropTable("dbo.Deputats");
        }
    }
}
