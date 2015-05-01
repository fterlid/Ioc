using System;
using Fte.Ioc.Registry;
using Fte.Ioc.Tests.Utils.TestServices;
using NUnit.Framework;

namespace Fte.Ioc.Tests.Registry
{
	[TestFixture]
	public class TypeRegistryItemTest
	{
		[Test]
		public void Ctor_AbstractionTypeAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() => new TypeRegistryItem(null, typeof (TestService), LifeCycle.Singleton));
			StringAssert.Contains("abstractionType", ex.Message);
		}

		[Test]
		public void Ctor_ConcreteTypeAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() => new TypeRegistryItem(typeof (ITestService), null, LifeCycle.Singleton));
			StringAssert.Contains("concreteType", ex.Message);
		}
	}
}
