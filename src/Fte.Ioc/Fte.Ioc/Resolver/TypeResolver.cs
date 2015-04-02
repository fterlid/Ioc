using Fte.Ioc.Exceptions;
using System;
using Fte.Ioc.Registry;

namespace Fte.Ioc.Resolver
{
	internal class TypeResolver : ITypeResolver
	{
		private readonly ITypeRegistry _typeRegistry;
		private readonly IObjectFactory _objectFactory;

		public TypeResolver(ITypeRegistry typeRegistry, IObjectFactory objectFactory)
		{
			if (typeRegistry == null) throw new ArgumentNullException("typeRegistry");
			if (objectFactory == null) throw new ArgumentNullException("objectFactory");

			_typeRegistry = typeRegistry;
			_objectFactory = objectFactory;
		}

		public object Resolve(Type typeToResolve)
		{
			var registryItem = _typeRegistry.GetRegistryItem(typeToResolve);

			return _objectFactory.Create(registryItem.ConcreteType);
		}
	}
}
