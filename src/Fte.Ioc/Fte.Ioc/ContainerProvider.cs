using Fte.Ioc.Facade;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;

namespace Fte.Ioc
{
	public static class ContainerProvider
	{
		public static IContainer GetContainer()
		{
			var typeRegistry = new TypeRegistry();
			var objectFactory = new ObjectFactory();
			var typeResolver = new TypeResolver(typeRegistry, objectFactory);

			return new Container(typeRegistry, typeResolver);
		}
	}
}
