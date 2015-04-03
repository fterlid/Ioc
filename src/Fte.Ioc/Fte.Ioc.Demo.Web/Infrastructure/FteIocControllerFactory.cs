using System;
using System.Web.Mvc;
using System.Web.Routing;
using Fte.Ioc.Facade;

namespace Fte.Ioc.Demo.Web.Infrastructure
{
	public class FteIocControllerFactory : DefaultControllerFactory
	{
		private IContainer _iocContainer;

		public FteIocControllerFactory(IContainer IocContainer)
		{
			_iocContainer = IocContainer;
		}

		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			return (IController) _iocContainer.Resolve(controllerType);
		}
	}
}