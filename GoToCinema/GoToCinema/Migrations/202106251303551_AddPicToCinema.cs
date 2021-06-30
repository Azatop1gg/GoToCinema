namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPicToCinema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cinemas", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cinemas", "Image");
        }
    }
}
