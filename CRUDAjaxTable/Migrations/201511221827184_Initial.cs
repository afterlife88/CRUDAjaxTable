namespace CRUDAjaxTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Operations", "Author_Id", "dbo.Authors");
            DropIndex("dbo.Operations", new[] { "Author_Id" });
            AlterColumn("dbo.Operations", "Author_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Operations", "Author_Id");
            AddForeignKey("dbo.Operations", "Author_Id", "dbo.Authors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "Author_Id", "dbo.Authors");
            DropIndex("dbo.Operations", new[] { "Author_Id" });
            AlterColumn("dbo.Operations", "Author_Id", c => c.Int());
            CreateIndex("dbo.Operations", "Author_Id");
            AddForeignKey("dbo.Operations", "Author_Id", "dbo.Authors", "Id");
        }
    }
}
