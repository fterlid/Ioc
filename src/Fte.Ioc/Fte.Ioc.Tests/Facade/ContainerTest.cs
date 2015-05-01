using System;
using Fte.Ioc.Facade;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils.TestServices;
using Moq;
using NUnit.Framework;

namespace Fte.Ioc.Tests.Facade
{
	[TestFixture]
	public class ContainerTest
	{
		private Mock<ITypeRegistry> _registryMock;
		private Mock<ITypeResolver> _resolverMock;
		private IContainer _container;

		[SetUp]
		public void Setup()
		{
			_registryMock = new Mock<ITypeRegistry>();
			_resolverMock = new Mock<ITypeResolver>();
			_container = new Container(_registryMock.Object, _resolverMock.Object);
		}

		[Test]
		public void Ctor_TypeRegistryIsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				new Container(null, _resolverMock.Object);
			});

			StringAssert.Contains("typeRegistry", ex.Message);
		}

		[Test]
		public void Ctor_TypeResolverIsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				new Container(_registryMock.Object, null);
			});

			StringAssert.Contains("typeResolver", ex.Message);
		}

		[Test]
		public void Discover_LifeCycleNotProvided_CallsServiceWithDefaultLifeCycle()
		{
			var assembly = GetType().Assembly;
			_container.Discover<ITestService>(assembly);
			_registryMock.Verify(x => x.Discover<ITestService>(assembly, _container.DefaultLifeCycle), Times.Once);
		}

		[Test]
		public void Discover_LifeCycleProvided_CallsServiceWithProvidedLifeCycle()
		{
			var assembly = GetType().Assembly;
			_container.Discover<ITestService>(assembly, LifeCycle.Singleton);
			_registryMock.Verify(x => x.Discover<ITestService>(assembly, LifeCycle.Singleton), Times.Once);
		}

		[Test]
		public void Register_GenericOneTypeWithoutParameter_CallsServiceWithDefaultLifeCycle()
		{
			_container.Register<TestService>();
			_registryMock.Verify(x => x.Register<TestService, TestService>(_container.DefaultLifeCycle), Times.Once);
		}

		[Test]
		public void Register_GenericOneTypeWithtParameter_CallsServiceWithCorrectLifeCycle()
		{
			_container.Register<TestService>(LifeCycle.Singleton);
			_registryMock.Verify(x => x.Register<TestService, TestService>(LifeCycle.Singleton), Times.Once);
		}

		[Test]
		public void Register_GenericTwoTypesWithoutParameter_CallsServiceWithDefaultLifeCycle()
		{
			_container.Register<ITestService, TestService>();
			_registryMock.Verify(x => x.Register<ITestService, TestService>(_container.DefaultLifeCycle), Times.Once);
		}

		[Test]
		public void Register_GenericTwoTypesWithParameter_CallsServiceWithCorrectLifeCycle()
		{
			_container.Register<ITestService, TestService>(LifeCycle.Singleton);
			_registryMock.Verify(x => x.Register<ITestService, TestService>(LifeCycle.Singleton), Times.Once);
		}

		[Test]
		public void Resolve_WhenCalled_CallsServiceWithCorrectType()
		{
			_container.Resolve(typeof(ITestService));
			_resolverMock.Verify(x => x.Resolve(typeof(ITestService)), Times.Once);
		}
	}
}
