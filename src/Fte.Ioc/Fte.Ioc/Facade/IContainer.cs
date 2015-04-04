using System;

namespace Fte.Ioc.Facade
{
	public interface IContainer
	{
		void Register<TConcrete>() where TConcrete : class;

		void Register<TConcrete>(LifeCycle lifeCycle) where TConcrete : class;

		void Register<TAbstraction, TConcrete>() where TConcrete : TAbstraction;

		void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction;

		object Resolve(Type typeToResolve);
    }
}
