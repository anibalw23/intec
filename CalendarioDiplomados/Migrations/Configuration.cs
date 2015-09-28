namespace CalendarioDiplomados.Migrations
{
    using CalendarioDiplomados.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CalendarioDiplomados.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CalendarioDiplomados.Models.ApplicationDbContext";
        }

        protected override void Seed(CalendarioDiplomados.Models.ApplicationDbContext context)
        {

            if (!context.Roles.Any(r => r.Name == "Administrador"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Administrador" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Visualizador"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Visualizador" };
                manager.Create(role);
            }

            if (!(context.Users.Any(u => u.UserName == "admin@admin.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "admin@admin.com", PhoneNumber = "0797697898" };
                userManager.Create(userToInsert, "adminIntec451065");
                userManager.AddToRole(userToInsert.Id, "Administrador");
            }


            if (!(context.Users.Any(u => u.UserName == "visual@visual.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "visual@visual.com", PhoneNumber = "0797697898" };
                userManager.Create(userToInsert, "visualIntec451065");
                userManager.AddToRole(userToInsert.Id, "Visualizador");
            }


        }
    }
}
