﻿using System;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;

namespace Fte.Ioc
{
	public class Container
	{
		private readonly ITypeRegistry _typeRegistry;
		private readonly ITypeResolver _typeResolver;

		internal Container(ITypeRegistry typeRegistry, ITypeResolver typeResolver)
		{
			_typeRegistry = typeRegistry;
			_typeResolver = typeResolver;
		}

		public void Register<TAbstraction, TConcrete>() where TConcrete : TAbstraction
		{
			Register<TAbstraction, TConcrete>(LifeCycle.Transient);
		}

		public void Register<TAbstraction, TConcrete>(LifeCycle lifeCycle) where TConcrete : TAbstraction
		{
			_typeRegistry.Register<TAbstraction, TConcrete>(lifeCycle);
		}

		public T Resolve<T>(Type typeToResolve) where T : class
		{
			return Resolve(typeToResolve) as T;
		}

		public object Resolve(Type typeToResolve)
		{
			return _typeResolver.Resolve(typeToResolve);
		}
	}
}
