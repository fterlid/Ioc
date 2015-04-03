using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fte.Ioc.Demo.Web.Startup))]
namespace Fte.Ioc.Demo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
