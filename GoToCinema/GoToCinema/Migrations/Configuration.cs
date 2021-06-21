namespace GoToCinema.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using GoToCinema.Models;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<GoToCinema.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GoToCinema.Models.ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore(context));

            var role1 = new Role { Name = "admin" };
            if (context.Roles.FirstOrDefault(x => x.Name.ToLower() == role1.Name) == null)
            {
                context.Roles.Add(role1);
            }

            var role2 = new Role { Name = "user" };
            if (context.Roles.FirstOrDefault(x => x.Name.ToLower() == role2.Name) == null)
            {
                context.Roles.Add(role2);
            }

            var admin = new ApplicationUser
            {
                Email = "admin@admin.kg",
                UserName = "admin@admin.kg",
                EmailConfirmed = true,
            };
            var existedUser = userManager.FindByEmail(admin.Email);
            if (existedUser == null)
            {
                var result = userManager.Create(admin, "Admin1#");
                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, role1.Name);
                    userManager.AddToRole(admin.Id, role2.Name);
                }
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
