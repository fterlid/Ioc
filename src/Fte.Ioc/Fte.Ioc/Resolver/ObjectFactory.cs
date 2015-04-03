using Fte.Ioc.Registry;
using System;
using System.Collections.Generic;

namespace Fte.Ioc.Resolver
{
	internal class ObjectFactory : IObjectFactory
	{
		private readonly IDictionary<Type, object> _singletonObjects;

		public ObjectFactory()
		{
			_singletonObjects = new Dictionary<Type, object>();
		}

		public object GetInstance(TypeRegistryItem registryItem, object[] constructorParams)
		{
			if (registryItem == null) throw new ArgumentNullException("registryItem");
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
	}
}
