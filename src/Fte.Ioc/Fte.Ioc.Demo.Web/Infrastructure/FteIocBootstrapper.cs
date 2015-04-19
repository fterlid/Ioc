using System.Web.Mvc;
using Fte.Ioc.Demo.Services;
using Fte.Ioc.Facade;

namespace Fte.Ioc.Demo.Web.Infrastructure
{
	public static class FteIocBootstrapper
	{
		public static void Configure(IContainer container)
		{
			container.Discover<IController>(typeof(FteIocBootstrapper).Assembly);

			container.Register<ISomeOtherService, SomeOtherService>();
			container.Register<ISomeService, SomeService>(LifeCycle.Singleton);
		}
	}
}