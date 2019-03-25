namespace Web.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Web.Data.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Web.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Web.Data.DataContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole
                {
                    Name = "Admin"
                });

                var supervisorRole = new IdentityRole
                {
                    Name = "Supervisor"

                };
                context.Roles.Add(supervisorRole);

                var hasher = new PasswordHasher();
                var userAdmin = new ApplicationUser
                {
                    Email = "supervisor@clearskymaps.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hasher.HashPassword("VerySecurePassword"),
                    UserName = "supervisor@clearskymaps.com",
                };
                context.Users.Add(userAdmin);
                context.SaveChanges();

                var userRoleSet = context.Set<IdentityUserRole>();
                userRoleSet.AddOrUpdate(new IdentityUserRole
                {
                    RoleId = supervisorRole.Id,
                    UserId = userAdmin.Id
                });
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}
