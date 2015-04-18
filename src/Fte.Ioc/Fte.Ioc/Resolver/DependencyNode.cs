using System;
using Fte.Ioc.Registry;

namespace Fte.Ioc.Resolver
{
	internal class DependencyNode
	{
		public DependencyNode(TypeRegistryItem typeRegistryItem)
		{
			if (typeRegistryItem == null)
			{
				throw new ArgumentNullException(nameof(typeRegistryItem));
			}

			RegistryItem = typeRegistryItem;
		}

		public TypeRegistryItem RegistryItem { get; private set; }

		public bool Discovered { get; set; }
	}
}
