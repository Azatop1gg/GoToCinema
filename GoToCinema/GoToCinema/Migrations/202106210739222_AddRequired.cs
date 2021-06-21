namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cinemas", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Halls", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Genre", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Producer", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Country", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Country", c => c.String());
            AlterColumn("dbo.Movies", "Producer", c => c.String());
            AlterColumn("dbo.Movies", "Genre", c => c.String());
            AlterColumn("dbo.Movies", "Name", c => c.String());
            AlterColumn("dbo.Halls", "Name", c => c.String());
            AlterColumn("dbo.Cinemas", "Name", c => c.String());
        }
    }
}
