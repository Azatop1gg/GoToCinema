namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowNameString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rows", "RowName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rows", "RowName");
        }
    }
}
