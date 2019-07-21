using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sali_site.Startup))]
namespace sali_site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
