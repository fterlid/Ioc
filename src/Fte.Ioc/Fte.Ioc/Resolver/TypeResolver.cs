﻿using System;
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

			return EnsureAcyclicDependencyGraph(registryItem);
		}

		private IEnumerable<TypeRegistryItem> GetConstructorParameterItems(Type concreteType)
		{
			var constructorInfo = concreteType.GetConstructors().First();
			return constructorInfo.GetParameters().Select(p => _typeRegistry.GetRegistryItem(p.ParameterType));
		}

		private object EnsureAcyclicDependencyGraph(TypeRegistryItem typeRegistryItem)
		{
			// Instead of maintaining a topological sort of the nodes in the dependency tree,
			// instances of the nodes are stored in a dictionary.
			var dependencyInstances = new Dictionary<TypeRegistryItem, object>();

			var dfsStack = new Stack<DependencyNode>();
			dfsStack.Push(new DependencyNode(typeRegistryItem));

			while (dfsStack.Count > 0)
			{
				var current = dfsStack.Peek();
				if (!current.Discovered)
				{
					var children = GetConstructorParameterItems(current.RegistryItem.ConcreteType);
					foreach (var child in children)
					{
						if (dfsStack.Any(n => n.RegistryItem.AbstractionType == child.AbstractionType))
						{
							throw new CircularDependencyException();
						}
						dfsStack.Push(new DependencyNode(child));
					}
					current.Discovered = true;
				}
				else
				{
					dfsStack.Pop();
					dependencyInstances[current.RegistryItem] = GetObject(current.RegistryItem, dependencyInstances);
				}
			}

			return dependencyInstances[typeRegistryItem];
		}

		private object GetObject(TypeRegistryItem itemToInstantiate, Dictionary<TypeRegistryItem, object> dependencyInstances)
		{
			if (_objectManager.HasInstance(itemToInstantiate))
			{
				return _objectManager.GetInstance(itemToInstantiate);
			}

			var ctorParameterItems = GetConstructorParameterItems(itemToInstantiate.ConcreteType);
			return _objectManager.Create(itemToInstantiate, ctorParameterItems.Select(regItem => dependencyInstances[regItem]).ToArray());
		}
	}
}
