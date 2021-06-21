using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoToCinema.Startup))]
namespace GoToCinema
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
