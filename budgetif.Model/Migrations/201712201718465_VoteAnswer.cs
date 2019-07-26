namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoteAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "Answer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "Answer");
        }
    }
}
