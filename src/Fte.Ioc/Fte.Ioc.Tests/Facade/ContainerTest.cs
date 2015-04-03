using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Fte.Ioc.Facade.Tests
{
	[TestClass]
	public class ContainerTest
	{
		private Mock<ITypeRegistry> _registryMock;
		private Mock<ITypeResolver> _resolverMock;
		private IContainer _container;

		[TestInitialize]
		public void TestInitialize()
		{
			_registryMock = new Mock<ITypeRegistry>();
			_resolverMock = new Mock<ITypeResolver>();
			_container = new Container(_registryMock.Object, _resolverMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_TypeRegistryIsNull_ThrowsException()
		{
			var container = new Container(null, _resolverMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_TypeResolverIsNull_ThrowsException()
		{
			var container = new Container(_registryMock.Object, null);
		}

		[TestMethod]
		public void Register_WithoutParameter_CallsServiceWithTransientLifeCycle()
		{
			_container.Register<ITestService, TestService>();

			_registryMock.Verify(x => x.Register<ITestService, TestService>(LifeCycle.Transient), Times.Once);
		}

		[TestMethod]
		public void Register_WithtParameter_CallsServiceWithCorrectLifeCycle()
		{
			_container.Register<ITestService, TestService>(LifeCycle.Singleton);

			_registryMock.Verify(x => x.Register<ITestService, TestService>(LifeCycle.Singleton), Times.Once);
		}

		[TestMethod]
		public void Resolve_Unparameterized_CallsServiceWithCorrectType()
		{
			_container.Resolve(typeof(ITestService));

			_resolverMock.Verify(x => x.Resolve(typeof(ITestService)), Times.Once);
		}
	}
}
