using Fte.Ioc.Registry;

namespace Fte.Ioc.Resolver
{
	internal interface IObjectManager
	{
		bool HasInstance(TypeRegistryItem registryItem);

		object GetInstance(TypeRegistryItem registryItem);

		object Create(TypeRegistryItem registryItem, object[] constructorParams);
	}
}
