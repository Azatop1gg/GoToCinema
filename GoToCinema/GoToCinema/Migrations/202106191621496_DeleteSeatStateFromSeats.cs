namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSeatStateFromSeats : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Seats", "SeatState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "SeatState", c => c.Int(nullable: false));
        }
    }
}
