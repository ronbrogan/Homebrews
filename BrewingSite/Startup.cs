using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BrewingSite.Startup))]
namespace BrewingSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
