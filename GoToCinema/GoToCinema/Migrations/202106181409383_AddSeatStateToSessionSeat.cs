namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeatStateToSessionSeat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rows", "RowName", c => c.String());
            DropColumn("dbo.Rows", "RowNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rows", "RowNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Rows", "RowName");
        }
    }
}
