namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    public partial class AddImageToMovie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Image");
        }
    }
}
