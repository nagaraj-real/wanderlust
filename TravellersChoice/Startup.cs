using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TravellersChoice.Startup))]
namespace TravellersChoice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
