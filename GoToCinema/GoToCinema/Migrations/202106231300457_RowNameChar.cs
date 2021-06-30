namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowNameChar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rows", "RowName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rows", "RowName", c => c.String(nullable: false));
        }
    }
}
