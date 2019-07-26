namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixChatUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Deputats", "ChatUser_Id", "dbo.ChatUsers");
            DropIndex("dbo.Deputats", new[] { "ChatUser_Id" });
            CreateTable(
                "dbo.DeputatChatUsers",
                c => new
                    {
                        Deputat_Id = c.Int(nullable: false),
                        ChatUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Deputat_Id, t.ChatUser_Id })
                .ForeignKey("dbo.Deputats", t => t.Deputat_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChatUsers", t => t.ChatUser_Id, cascadeDelete: true)
                .Index(t => t.Deputat_Id)
                .Index(t => t.ChatUser_Id);
            
            AlterColumn("dbo.ChatUsers", "TelegramId", c => c.Long(nullable: false));
            DropColumn("dbo.Deputats", "ChatUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deputats", "ChatUser_Id", c => c.Int());
            DropForeignKey("dbo.DeputatChatUsers", "ChatUser_Id", "dbo.ChatUsers");
            DropForeignKey("dbo.DeputatChatUsers", "Deputat_Id", "dbo.Deputats");
            DropIndex("dbo.DeputatChatUsers", new[] { "ChatUser_Id" });
            DropIndex("dbo.DeputatChatUsers", new[] { "Deputat_Id" });
            AlterColumn("dbo.ChatUsers", "TelegramId", c => c.Int(nullable: false));
            DropTable("dbo.DeputatChatUsers");
            CreateIndex("dbo.Deputats", "ChatUser_Id");
            AddForeignKey("dbo.Deputats", "ChatUser_Id", "dbo.ChatUsers", "Id");
        }
    }
}
