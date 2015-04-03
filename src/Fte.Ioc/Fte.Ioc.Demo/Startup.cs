using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fte.Ioc.Demo.Startup))]
namespace Fte.Ioc.Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
