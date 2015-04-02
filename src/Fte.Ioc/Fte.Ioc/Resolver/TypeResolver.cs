using Fte.Ioc.Exceptions;
using System;
using Fte.Ioc.Registry;

namespace Fte.Ioc.Resolver
{
	internal class TypeResolver : ITypeResolver
	{
		private ITypeRegistry _typeRegistry;

		public TypeResolver(ITypeRegistry typeRegistry)
		{
			if (typeRegistry == null) throw new ArgumentNullException("typeRegistry");

			_typeRegistry = typeRegistry;
		}

		public object Resolve(Type typeToResolve)
		{
			var registryItem = _typeRegistry.GetRegistryItem(typeToResolve);

			return Activator.CreateInstance(registryItem.ConcreteType);
		}
	}
}
