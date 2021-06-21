namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSessionSeat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rows", "Session_Id", "dbo.Sessions");
            DropIndex("dbo.Rows", new[] { "Session_Id" });
            DropColumn("dbo.Rows", "Session_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rows", "Session_Id", c => c.Int());
            CreateIndex("dbo.Rows", "Session_Id");
            AddForeignKey("dbo.Rows", "Session_Id", "dbo.Sessions", "Id");
        }
    }
}
