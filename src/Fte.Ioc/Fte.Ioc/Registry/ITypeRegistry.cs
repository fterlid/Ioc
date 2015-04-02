using System;

namespace Fte.Ioc.Registry
{
	internal interface ITypeRegistry
	{
		void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle);

		TypeRegistryItem GetRegistryItem(Type abstractionType);
	}
}
