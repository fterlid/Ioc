using System;
using System.Reflection;

namespace Fte.Ioc.Facade
{
	public interface IContainer
	{
		void Discover<T>(Assembly assembly) where T : class;

		void Discover<T>(Assembly assembly, LifeCycle lifeCycle) where T : class;

		void Register<TConcrete>() where TConcrete : class;

		void Register<TConcrete>(LifeCycle lifeCycle) where TConcrete : class;

		void Register<TAbstraction, TConcrete>() where TConcrete : TAbstraction;

		void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction;

		object Resolve(Type typeToResolve);
	}
}
