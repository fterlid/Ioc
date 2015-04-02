using System;

namespace Fte.Ioc
{
	public class Container
	{
		public void Register<TAbstraction, TConcrete>() where TConcrete : TAbstraction
		{
			throw new NotImplementedException();
		}

		public void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction
		{
			throw new NotImplementedException();
		}

		public T Resolve<T>(Type typeToResolve) where T : class
		{
			throw new NotImplementedException();
		}

		public object Resolve(Type typeToResolve)
		{
			throw new NotImplementedException();
		}
	}
}
