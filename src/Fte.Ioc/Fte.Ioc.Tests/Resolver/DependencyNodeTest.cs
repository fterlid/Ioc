using System;
using Fte.Ioc.Resolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fte.Ioc.Tests.Resolver
{
	[TestClass]
	public class DependencyNodeTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ctor_RegistryItemAsNull_ThrowsException()
		{
			var node = new DependencyNode(null);
		}
	}
}
