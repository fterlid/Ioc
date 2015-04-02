using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Fte.Ioc.Tests.Resolver
{
	[TestClass]
	public class TypeResolverTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_TypeRegistryAsNull_ThrowsException()
		{
			new TypeResolver(null);
		}

		[TestMethod]
		public void Resolve_InputTypeIsRegistered_ReturnsObjectOfInputType()
		{
			var registryItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			var registryMock = new Mock<ITypeRegistry>();
			registryMock.Setup(x => x.GetRegistryItem(It.IsAny<Type>())).Returns(registryItem);
			var resolver = new TypeResolver(registryMock.Object);

			var obj = resolver.Resolve(typeof(ITestService));

			Assert.IsInstanceOfType(obj, typeof(ITestService));
		}
	}
}
