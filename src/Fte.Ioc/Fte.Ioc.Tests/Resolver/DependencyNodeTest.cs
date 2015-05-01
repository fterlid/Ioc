using System;
using Fte.Ioc.Resolver;
using NUnit.Framework;

namespace Fte.Ioc.Tests.Resolver
{
	[TestFixture]
	public class DependencyNodeTest
	{
		[Test]
		public void ctor_RegistryItemAsNull_ThrowsException()
		{
			Assert.Catch<ArgumentNullException>(() =>
			{
				new DependencyNode(null);
			});
		}
	}
}
