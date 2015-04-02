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
			factory.Create(null, new object[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_ConstructorParamsArrayAsNull_ThrowsException()
		{
			var factory = new ObjectFactory();
			factory.Create(typeof(TestService), null);
		}

		[TestMethod]
		public void Create_ConcreteTypeAsInput_ReturnsObjectOfInputType()
		{
			var factory = new ObjectFactory();

			var obj = factory.Create(typeof(TestService), new object[0]);

			Assert.IsInstanceOfType(obj, typeof(TestService));
		}
	}
}
