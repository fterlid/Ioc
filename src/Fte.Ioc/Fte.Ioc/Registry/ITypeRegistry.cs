using System;

namespace Fte.Ioc.Registry
{
	internal interface ITypeRegistry
	{
		void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction;

		TypeRegistryItem GetRegistryItem(Type abstractionType);
	}
}
