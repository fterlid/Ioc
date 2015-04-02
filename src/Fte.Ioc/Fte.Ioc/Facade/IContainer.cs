using System;

namespace Fte.Ioc.Facade
{
	public interface IContainer
	{
		void Register<TAbstraction, TConcrete>() where TConcrete : TAbstraction;

		void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction;

		T Resolve<T>(Type typeToResolve) where T : class;

		object Resolve(Type typeToResolve);
    }
}
