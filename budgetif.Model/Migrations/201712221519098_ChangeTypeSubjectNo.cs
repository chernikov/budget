namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeSubjectNo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Polls", "SubjectNo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Polls", "SubjectNo", c => c.Int(nullable: false));
        }
    }
}
