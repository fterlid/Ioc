using Fte.Ioc.Demo.Services;
using Fte.Ioc.Demo.Web.Controllers;
using Fte.Ioc.Facade;

namespace Fte.Ioc.Demo.Web.Infrastructure
{
	public static class FteIocBootstrapper
	{
		public static void Configure(IContainer container)
		{
			container.Register<HomeController, HomeController>();
			container.Register<ISomeOtherService, SomeOtherService>();
			container.Register<ISomeService, SomeService>(LifeCycle.Singleton);
		}
	}
}