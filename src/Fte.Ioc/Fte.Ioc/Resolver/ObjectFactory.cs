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

		public object Create(TypeRegistryItem registryItem, object[] constructorParams)
		{
			if (registryItem == null) throw new ArgumentNullException("registryItem");
			if (constructorParams == null) throw new ArgumentNullException("constructorParams");

			var concreteType = registryItem.ConcreteType;

			if (_singletonObjects.ContainsKey(concreteType))
			{
				return _singletonObjects[concreteType];
			}

			var instance = Activator.CreateInstance(concreteType, constructorParams);

			if (registryItem.LifeCycle == LifeCycle.Singleton)
			{
				_singletonObjects.Add(concreteType, instance);
			}

			return instance;
		}
	}
}
