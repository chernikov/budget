namespace budgetif.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        payer_edrpou = c.String(),
                        payer_name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        recipt_edrpou = c.String(),
                        recipt_name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        number = c.Int(nullable: false),
                        doc_number = c.String(),
                        doc_date = c.DateTime(nullable: false),
                        doc_v_date = c.DateTime(nullable: false),
                        trans_date = c.DateTime(nullable: false),
                        amount = c.Double(nullable: false),
                        payerId = c.Int(nullable: false),
                        receiptId = c.Int(nullable: false),
                        payment_details = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Payers", t => t.payerId, cascadeDelete: true)
                .ForeignKey("dbo.Receipts", t => t.receiptId, cascadeDelete: true)
                .Index(t => t.number, unique: true)
                .Index(t => t.payerId)
                .Index(t => t.receiptId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "receiptId", "dbo.Receipts");
            DropForeignKey("dbo.Transactions", "payerId", "dbo.Payers");
            DropIndex("dbo.Transactions", new[] { "receiptId" });
            DropIndex("dbo.Transactions", new[] { "payerId" });
            DropIndex("dbo.Transactions", new[] { "number" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Receipts");
            DropTable("dbo.Payers");
        }
    }
}
