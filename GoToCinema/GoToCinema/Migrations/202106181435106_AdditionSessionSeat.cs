namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionSessionSeat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Halls", "CinemaId", "dbo.Cinemas");
            DropForeignKey("dbo.Rows", "HallId", "dbo.Halls");
            DropForeignKey("dbo.Sessions", "HallId", "dbo.Halls");
            DropForeignKey("dbo.Seats", "RowId", "dbo.Rows");
            DropForeignKey("dbo.Sessions", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            CreateTable(
                "dbo.SessionSeats",
                c => new
                    {
                        RowId = c.Int(nullable: false),
                        SeatId = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                        SeatState = c.Int(nullable: false),
                        UserId = c.Int(),
                        ApplicationUser_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.RowId, t.SeatId, t.SessionId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Rows", t => t.RowId)
                .ForeignKey("dbo.Seats", t => t.SeatId)
                .ForeignKey("dbo.Sessions", t => t.SessionId)
                .Index(t => t.RowId)
                .Index(t => t.SeatId)
                .Index(t => t.SessionId)
                .Index(t => t.ApplicationUser_Id);
            
            AddForeignKey("dbo.Halls", "CinemaId", "dbo.Cinemas", "Id");
            AddForeignKey("dbo.Rows", "HallId", "dbo.Halls", "Id");
            AddForeignKey("dbo.Sessions", "HallId", "dbo.Halls", "Id");
            AddForeignKey("dbo.Seats", "RowId", "dbo.Rows", "Id");
            AddForeignKey("dbo.Sessions", "MovieId", "dbo.Movies", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Sessions", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Seats", "RowId", "dbo.Rows");
            DropForeignKey("dbo.Sessions", "HallId", "dbo.Halls");
            DropForeignKey("dbo.Rows", "HallId", "dbo.Halls");
            DropForeignKey("dbo.Halls", "CinemaId", "dbo.Cinemas");
            DropForeignKey("dbo.SessionSeats", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.SessionSeats", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.SessionSeats", "RowId", "dbo.Rows");
            DropForeignKey("dbo.SessionSeats", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SessionSeats", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SessionSeats", new[] { "SessionId" });
            DropIndex("dbo.SessionSeats", new[] { "SeatId" });
            DropIndex("dbo.SessionSeats", new[] { "RowId" });
            DropTable("dbo.SessionSeats");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sessions", "MovieId", "dbo.Movies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Seats", "RowId", "dbo.Rows", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sessions", "HallId", "dbo.Halls", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Rows", "HallId", "dbo.Halls", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Halls", "CinemaId", "dbo.Cinemas", "Id", cascadeDelete: true);
        }
    }
}
