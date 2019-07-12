using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SALIVOSA.Startup))]
namespace SALIVOSA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
