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
			var objectFactoryMock = new Mock<IObjectFactory>();
			new TypeResolver(null, objectFactoryMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_ObjectFactoryAsNull_ThrowsException()
		{
			var registryMock = new Mock<ITypeRegistry>();
			new TypeResolver(registryMock.Object, null);
		}

		[TestMethod]
		public void Resolve_InputTypeIsRegistered_ObjectIsCreated()
		{
			var registryItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			var registryMock = new Mock<ITypeRegistry>();
			registryMock.Setup(x => x.GetRegistryItem(It.IsAny<Type>())).Returns(registryItem);
			var objectFactoryMock = new Mock<IObjectFactory>();
            var resolver = new TypeResolver(registryMock.Object, objectFactoryMock.Object);

			resolver.Resolve(typeof(ITestService));

			objectFactoryMock.Verify(x => x.Create(typeof(TestService)), Times.Once);
		}
	}
}
