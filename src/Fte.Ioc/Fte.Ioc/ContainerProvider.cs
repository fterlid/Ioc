using Fte.Ioc.Facade;
using Fte.Ioc.ObjectManagement;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;

namespace Fte.Ioc
{
	public static class ContainerProvider
	{
		public static IContainer GetContainer()
		{
			var typeRegistry = new TypeRegistry();
			var objectManager = new ObjectManager();
			var typeResolver = new TypeResolver(typeRegistry, objectManager);

			return new Container(typeRegistry, typeResolver);
		}
	}
}
