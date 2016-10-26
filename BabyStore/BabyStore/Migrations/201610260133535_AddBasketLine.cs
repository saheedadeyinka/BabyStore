namespace BabyStore.Migrations.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBasketLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasketLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BasketId = c.String(),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketLines", "ProductId", "dbo.Products");
            DropIndex("dbo.BasketLines", new[] { "ProductId" });
            DropTable("dbo.BasketLines");
        }
    }
}
