using Fte.Ioc.Demo.Web.Infrastructure;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Fte.Ioc.Demo.Web
{
	public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			var container = ContainerProvider.GetContainer();
			FteIocBootstrapper.Configure(container);

			var controllerFactory = new FteIocControllerFactory(container);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
