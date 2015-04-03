using Fte.Ioc.Registry;

namespace Fte.Ioc.Resolver
{
	internal interface IObjectFactory
	{
		object Create(TypeRegistryItem registryItem, object[] constructorParams);
	}
}
