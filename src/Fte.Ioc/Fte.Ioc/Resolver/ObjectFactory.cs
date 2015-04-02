using System;

namespace Fte.Ioc.Resolver
{
	internal class ObjectFactory : IObjectFactory
	{
		public object Create(Type concreteType)
		{
			if (concreteType == null) throw new ArgumentNullException("concreteType");

			return Activator.CreateInstance(concreteType);
		}
	}
}
