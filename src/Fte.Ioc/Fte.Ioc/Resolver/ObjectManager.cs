using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using System;
using System.Collections.Generic;

namespace Fte.Ioc.Resolver
{
	internal class ObjectManager : IObjectManager
	{
		private readonly IDictionary<Type, object> _singletonObjects;

		public ObjectManager()
		{
			_singletonObjects = new Dictionary<Type, object>();
		}

		public bool HasInstance(TypeRegistryItem registryItem)
		{
			AssertTypeRegistryItem(registryItem);

			return registryItem.LifeCycle == LifeCycle.Singleton 
				&& _singletonObjects.ContainsKey(registryItem.ConcreteType);
		}

		public object GetInstance(TypeRegistryItem registryItem)
		{
			if (HasInstance(registryItem))
			{
				return _singletonObjects[registryItem.ConcreteType];
			}

			throw new ObjectNotCreatedException("Singleton object of type {0} has not been instantiated yet. ");
		}

		public object Create(TypeRegistryItem registryItem, object[] constructorParams)
		{
			AssertTypeRegistryItem(registryItem);
			if (constructorParams == null) throw new ArgumentNullException("constructorParams");

			var concreteType = registryItem.ConcreteType;

			if (_singletonObjects.ContainsKey(concreteType))
			{
				return _singletonObjects[concreteType];
			}

			return Create(concreteType, constructorParams, registryItem.LifeCycle);
		}

		private object Create(Type concreteType, object[] constructorParams, LifeCycle lifeCycle)
		{
			var instance = Activator.CreateInstance(concreteType, constructorParams);

			if (lifeCycle == LifeCycle.Singleton)
			{
				_singletonObjects.Add(concreteType, instance);
			}

			return instance;
		}

		private void AssertTypeRegistryItem(TypeRegistryItem registryItem)
		{
			if (registryItem == null)
			{
				throw new ArgumentNullException("registryItem");
			}
		}
	}
}
