using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CalendarioDiplomados.Startup))]
namespace CalendarioDiplomados
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
