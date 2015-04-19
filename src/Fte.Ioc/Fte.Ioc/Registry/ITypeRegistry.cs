using System;
using System.Reflection;

namespace Fte.Ioc.Registry
{
	internal interface ITypeRegistry
	{
		void Discover<T>(Assembly assembly, LifeCycle lifeCycle) where T : class;

		void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction;

		TypeRegistryItem GetRegistryItem(Type abstractionType);
	}
}
