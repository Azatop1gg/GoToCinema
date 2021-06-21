namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rows", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rows", "Note");
        }
    }
}
