using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fte.Ioc.Tests.Resolver
{
	[TestClass]
	public class ObjectFactoryTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_InputTypeAsNull_ThrowsException()
		{
			var factory = new ObjectFactory();
			factory.Create(null);
		}

		[TestMethod]
		public void Create_ConcreteTypeAsInput_ReturnsObjectOFInputType()
		{
			var factory = new ObjectFactory();

			var obj = factory.Create(typeof(TestService));

			Assert.IsInstanceOfType(obj, typeof(TestService));
		}
	}
}
