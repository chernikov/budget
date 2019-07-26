namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TelegramId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Deputats", "ChatUser_Id", c => c.Int());
            CreateIndex("dbo.Deputats", "ChatUser_Id");
            AddForeignKey("dbo.Deputats", "ChatUser_Id", "dbo.ChatUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deputats", "ChatUser_Id", "dbo.ChatUsers");
            DropIndex("dbo.Deputats", new[] { "ChatUser_Id" });
            DropColumn("dbo.Deputats", "ChatUser_Id");
            DropTable("dbo.ChatUsers");
        }
    }
}
