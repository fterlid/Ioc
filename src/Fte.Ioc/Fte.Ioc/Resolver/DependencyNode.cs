using Fte.Ioc.Registry;

namespace Fte.Ioc.Resolver
{
	internal class DependencyNode
	{
		public TypeRegistryItem RegistryItem { get; set; }
		public bool Discovered { get; set; }
	}
}
