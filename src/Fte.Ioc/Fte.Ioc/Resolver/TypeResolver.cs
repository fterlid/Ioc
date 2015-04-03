using System;
using Fte.Ioc.Registry;
using System.Collections.Generic;
using System.Linq;

namespace Fte.Ioc.Resolver
{
	internal class TypeResolver : ITypeResolver
	{
		private readonly ITypeRegistry _typeRegistry;
		private readonly IObjectFactory _objectFactory;
		private readonly IDictionary<Type, object> _singletonObjects;

		public TypeResolver(ITypeRegistry typeRegistry, IObjectFactory objectFactory)
		{
			if (typeRegistry == null) throw new ArgumentNullException("typeRegistry");
			if (objectFactory == null) throw new ArgumentNullException("objectFactory");

			_typeRegistry = typeRegistry;
			_objectFactory = objectFactory;
			_singletonObjects = new Dictionary<Type, object>();
        }

		public object Resolve(Type typeToResolve)
		{
			var registryItem = _typeRegistry.GetRegistryItem(typeToResolve);
			var constructorParamObjects = ResolveConstructorParameters(registryItem.ConcreteType);
			var resolvedObject = _objectFactory.GetInstance(registryItem, constructorParamObjects.ToArray());

			return resolvedObject;
		}

		private IEnumerable<object> ResolveConstructorParameters(Type concreteType)
		{
			var parameterTypes = GetConstructorParameterTypes(concreteType);
			foreach (var type in parameterTypes)
			{
				yield return Resolve(type);
			}
		}

		private IEnumerable<Type> GetConstructorParameterTypes(Type concreteType)
		{
			var constructorInfo = concreteType.GetConstructors().First();
			foreach(var p in constructorInfo.GetParameters())
			{
				yield return p.ParameterType;
			}
		}
	}
}
