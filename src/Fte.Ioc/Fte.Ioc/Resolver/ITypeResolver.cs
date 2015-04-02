using System;

namespace Fte.Ioc.Resolver
{
	internal interface ITypeResolver
	{
		object Resolve(Type typeToResolve);
	}
}
