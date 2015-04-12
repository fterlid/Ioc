using System;
using Fte.Ioc.Registry;
using System.Collections.Generic;
using System.Linq;

namespace Fte.Ioc.Resolver
{
	internal class TypeResolver : ITypeResolver
	{
		private readonly ITypeRegistry _typeRegistry;
		private readonly IObjectManager _objectManager;
		private readonly IDictionary<Type, object> _singletonObjects;

		public TypeResolver(ITypeRegistry typeRegistry, IObjectManager objectManager)
		{
			if (typeRegistry == null) throw new ArgumentNullException("typeRegistry");
			if (objectManager == null) throw new ArgumentNullException("objectFactory");

			_typeRegistry = typeRegistry;
			_objectManager = objectManager;
			_singletonObjects = new Dictionary<Type, object>();
        }

		public object Resolve(Type typeToResolve)
		{
			if (typeToResolve == null) throw new ArgumentNullException("typeToResolve");

			var registryItem = _typeRegistry.GetRegistryItem(typeToResolve);

			if (_objectManager.HasInstance(registryItem))
			{
				return _objectManager.GetInstance(registryItem);
			}

			var constructorParamObjects = ResolveConstructorParameters(registryItem.ConcreteType);
			var resolvedObject = _objectManager.Create(registryItem, constructorParamObjects.ToArray());

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
