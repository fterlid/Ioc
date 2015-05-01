using System;
using System.Reflection;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;

namespace Fte.Ioc.Facade
{
	internal class Container : IContainer
	{
		private readonly ITypeRegistry _typeRegistry;
		private readonly ITypeResolver _typeResolver;

		public Container(ITypeRegistry typeRegistry, ITypeResolver typeResolver)
		{
			if (typeRegistry == null) throw new ArgumentNullException(nameof(typeRegistry));
			if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));

			_typeRegistry = typeRegistry;
			_typeResolver = typeResolver;
		}

		public LifeCycle DefaultLifeCycle => LifeCycle.Transient;

		public void Discover<T>(Assembly assembly) where T : class
		{
			Discover<T>(assembly, DefaultLifeCycle);
		}

		public void Discover<T>(Assembly assembly, LifeCycle lifeCycle) where T : class
		{
			_typeRegistry.Discover<T>(assembly, lifeCycle);
		}

		public void Register<TConcrete>() where TConcrete : class
		{
			Register<TConcrete, TConcrete>(DefaultLifeCycle);
		}

		public void Register<TConcrete>(LifeCycle lifeCycle) where TConcrete : class
		{
			Register<TConcrete, TConcrete>(lifeCycle);
		}

		public void Register<TAbstraction, TConcrete>() where TConcrete : TAbstraction
		{
			Register<TAbstraction, TConcrete>(DefaultLifeCycle);
		}

		public void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction
		{
			_typeRegistry.Register<TAbstraction, TConcrete>(lifeCycle);
		}

		public object Resolve(Type typeToResolve)
		{
			return _typeResolver.Resolve(typeToResolve);
		}
	}
}
