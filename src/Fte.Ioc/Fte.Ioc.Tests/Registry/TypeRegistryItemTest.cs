using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fte.Ioc.Registry;
using Fte.Ioc.Tests.Utils.TestServices;

namespace Fte.Ioc.Tests.Registry
{
	[TestClass]
	public class TypeRegistryItemTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_AbstractionTypeAsNull_ThrowsException()
		{
			var item = new TypeRegistryItem(null, typeof(TestService), LifeCycle.Singleton);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_ConcreteTypeAsNull_ThrowsException()
		{
			var item = new TypeRegistryItem(typeof(ITestService), null, LifeCycle.Singleton);
		}
	}
}
