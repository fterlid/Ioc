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
		private Mock<ITypeRegistry> _registryMock;
		private Mock<IObjectManager> _objectManagerMock;
		private ITypeResolver _resolver;

		[TestInitialize]
		public void TestInitialize()
		{
			_registryMock = new Mock<ITypeRegistry>();
			_objectManagerMock = new Mock<IObjectManager>();
			_resolver = new TypeResolver(_registryMock.Object, _objectManagerMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_TypeRegistryAsNull_ThrowsException()
		{
			new TypeResolver(null, _objectManagerMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_ObjectFactoryAsNull_ThrowsException()
		{
			new TypeResolver(_registryMock.Object, null);
		}

		[TestMethod]
		public void Resolve_TypeIsRegisteredAndHasDefaultConstructor_ObjectIsResolved()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(ITestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Once);
		}

		[TestMethod]
		public void Resolve_TypeIsRegisteredAndHasRegisteredDependency_ObjectIsResolved()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			RegisterType(typeof(IOtherTestService), typeof(OtherTestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(IOtherTestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Exactly(2));
		}

		[TestMethod]
		public void Resolve_SingeltonWithTransientDependency_NoObjectsAreCreatedIfSingletonHasBeenCreated()
		{
			RegisterType(typeof(IOtherTestService), typeof(OtherTestService), LifeCycle.Singleton);
			_objectManagerMock.Setup(x => x.HasInstance(It.IsAny<TypeRegistryItem>())).Returns(true);

			_resolver.Resolve(typeof(IOtherTestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Never);
		}

		private void RegisterType(Type abstraction, Type concrete, LifeCycle lifeCycle)
		{
			var testServiceItem = new TypeRegistryItem(abstraction, concrete, lifeCycle);
			_registryMock.Setup(x => x.GetRegistryItem(abstraction)).Returns(testServiceItem);
		}
	}
}
