﻿using System;
using Fte.Ioc.Registry;
using System.Collections.Generic;
using System.Linq;
using Fte.Ioc.Exceptions;
using Fte.Ioc.ObjectManagement;

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

			return GetDependencyGraphRootInstance(registryItem);
		}

		private object GetDependencyGraphRootInstance(TypeRegistryItem typeRegistryItem)
		{
			var dependencyInstances = new Dictionary<TypeRegistryItem, object>();

			var nodeStack = new Stack<DependencyNode>();
			nodeStack.Push(new DependencyNode(typeRegistryItem));

			while (nodeStack.Count > 0)
			{
				var current = nodeStack.Peek();
				if(current.Discovered)
				{
					nodeStack.Pop();
					dependencyInstances[current.RegistryItem] = GetInstance(current.RegistryItem, dependencyInstances);
				}
				else
				{
					var children = GetConstructorParameterItems(current.RegistryItem);
					foreach (var child in children)
					{
						if (nodeStack.Any(n => n.Discovered && n.RegistryItem.AbstractionType == child.AbstractionType))
						{
							throw new CircularDependencyException();
						}
						nodeStack.Push(new DependencyNode(child));
					}
					current.Discovered = true;
				}
			}

			return dependencyInstances[typeRegistryItem];
		}

		private IEnumerable<TypeRegistryItem> GetConstructorParameterItems(TypeRegistryItem registryItem)
		{
			var constructorInfo = registryItem.ConcreteType.GetConstructors().First();
			return constructorInfo.GetParameters().Select(p => _typeRegistry.GetRegistryItem(p.ParameterType));
		}

		private object GetInstance(TypeRegistryItem itemToInstantiate, IReadOnlyDictionary<TypeRegistryItem, object> dependencyInstances)
		{
			if (_objectManager.HasInstance(itemToInstantiate))
			{
				return _objectManager.GetInstance(itemToInstantiate);
			}

			var ctorParameterItems = GetConstructorParameterItems(itemToInstantiate);
			return _objectManager.Create(itemToInstantiate, ctorParameterItems.Select(regItem => dependencyInstances[regItem]).ToArray());
		}
	}
}
