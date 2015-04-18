using System;
using Fte.Ioc.Registry;
using System.Collections.Generic;
using System.Linq;
using Fte.Ioc.Exceptions;

namespace Fte.Ioc.Resolver
{
	internal class TypeResolver : ITypeResolver
	{
		private readonly ITypeRegistry _typeRegistry;
		private readonly IObjectManager _objectManager;

		public TypeResolver(ITypeRegistry typeRegistry, IObjectManager objectManager)
		{
			if (typeRegistry == null) throw new ArgumentNullException(nameof(typeRegistry));
			if (objectManager == null) throw new ArgumentNullException(nameof(objectManager));

			_typeRegistry = typeRegistry;
			_objectManager = objectManager;
        }

		public object Resolve(Type typeToResolve)
		{
			if (typeToResolve == null) throw new ArgumentNullException(nameof(typeToResolve));

			var registryItem = _typeRegistry.GetRegistryItem(typeToResolve);

			if (_objectManager.HasInstance(registryItem))
			{
				return _objectManager.GetInstance(registryItem);
			}

			EnsureAcyclicDependencyGraph(typeToResolve);

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

		private void EnsureAcyclicDependencyGraph(Type typeToResolve)
		{
			//TODO: Refactor

			var topologicallySortedTypes = new List<Type>();

			var dfsStack = new Stack<DependencyNode>();
			dfsStack.Push(new DependencyNode(_typeRegistry.GetRegistryItem(typeToResolve)));
			while (dfsStack.Count > 0)
			{
				var current = dfsStack.Peek();
				if (!current.Discovered)
				{
					var children = GetConstructorParameterTypes(current.RegistryItem.ConcreteType);
					foreach (var child in children)
					{
						if (dfsStack.Any(n => n.RegistryItem.AbstractionType == child))
						{
							throw new CircularDependencyException();
						}
						dfsStack.Push(new DependencyNode(_typeRegistry.GetRegistryItem(child)));
					}
					current.Discovered = true;
				}
				else
				{
					dfsStack.Pop();
					topologicallySortedTypes.Add(current.RegistryItem.AbstractionType);
					//TODO: Maintain topological sort of instances instead of types
				}
			}
		}
	}
}
