using System;

namespace Fte.Ioc.Resolver
{
	internal class ObjectFactory : IObjectFactory
	{
		public object Create(Type concreteType, object[] constructorParams)
		{
			if (concreteType == null) throw new ArgumentNullException("concreteType");
			if (constructorParams == null) throw new ArgumentNullException("constructorParams");

			return Activator.CreateInstance(concreteType, constructorParams);
		}
	}
}
