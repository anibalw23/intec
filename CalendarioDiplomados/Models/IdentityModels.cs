using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CalendarioDiplomados.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Diplomado> Diplomadoes { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Grupo> Grupoes { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Modulo> Moduloes { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Taller> Tallers { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Calendario> Calendarios { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.DiaSemana> DiaSemanas { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Evento> Eventoes { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Facilitador> Facilitadors { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Chofer> Chofers { get; set; }

        public System.Data.Entity.DbSet<CalendarioDiplomados.Models.Participante> Participantes { get; set; }
    }
}