using System;

namespace Fte.Ioc.Resolver
{
	internal interface IObjectFactory
	{
		object Create(Type concreteType);
	}
}
