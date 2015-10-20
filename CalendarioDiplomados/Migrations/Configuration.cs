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
        }

        protected override void Seed(CalendarioDiplomados.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //Crea el rol de Administrador
            if (!context.Roles.Any(r => r.Name == "Administrador"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Administrador" };
                manager.Create(role);
            }
            //Crea el rol de Visualizador
            if (!context.Roles.Any(r => r.Name == "Visualizador"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Visualizador" };
                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@admin.com" };

                manager.Create(user, "adminIntec451065");
                manager.AddToRole(user.Id, "Administrador");
            }

            if (!context.Users.Any(u => u.UserName == "visual@visual.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "visual@visual.com" };

                manager.Create(user, "visualIntec451065");
                manager.AddToRole(user.Id, "Visualizador");
            }
            
        }
    }
}
