namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductImageMappings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImageMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageNumber = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.ProductImages", t => t.ProductImageId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ProductImageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImageMappings", "ProductImageId", "dbo.ProductImages");
            DropForeignKey("dbo.ProductImageMappings", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductImageMappings", new[] { "ProductImageId" });
            DropIndex("dbo.ProductImageMappings", new[] { "ProductId" });
            DropTable("dbo.ProductImageMappings");
        }
    }
}
