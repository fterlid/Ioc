﻿using Fte.Ioc.Registry;
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
		private Mock<IObjectFactory> _objectFactoryMock;
		private ITypeResolver _resolver;

		[TestInitialize]
		public void TestInitialize()
		{
			_registryMock = new Mock<ITypeRegistry>();
			_objectFactoryMock = new Mock<IObjectFactory>();
			_resolver = new TypeResolver(_registryMock.Object, _objectFactoryMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_TypeRegistryAsNull_ThrowsException()
		{
			new TypeResolver(null, _objectFactoryMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_ObjectFactoryAsNull_ThrowsException()
		{
			new TypeResolver(_registryMock.Object, null);
		}

		[TestMethod]
		public void Resolve_TypeIsRegisteredAndHasDefaultConstructor_ObjectIsCreated()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(ITestService));

			_objectFactoryMock.Verify(x => x.Create(typeof(TestService), It.IsAny<object[]>()), Times.Once);
		}

		[TestMethod]
		public void Resolve_TypeIsRegisteredAndHasRegisteredDependency_ObjectIsCreated()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			RegisterType(typeof(IOtherTestService), typeof(OtherTestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(IOtherTestService));

			_objectFactoryMock.Verify(x => x.Create(typeof(TestService), It.IsAny<object[]>()), Times.Once);
			_objectFactoryMock.Verify(x => x.Create(typeof(OtherTestService), It.IsAny<object[]>()), Times.Once);
		}

		[TestMethod]
		public void Resolve_TypeHasSingletonLifeCycle_FactoryIsCalledOnce()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(ITestService));
			_resolver.Resolve(typeof(ITestService));

			_objectFactoryMock.Verify(x => x.Create(typeof(TestService), It.IsAny<object[]>()), Times.Once);
		}

		[TestMethod]
		public void Resolve_TypeHasTransientLifeCycle_FactoryIsCalledOneachResolve()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Transient);

			_resolver.Resolve(typeof(ITestService));
			_resolver.Resolve(typeof(ITestService));

			_objectFactoryMock.Verify(x => x.Create(typeof(TestService), It.IsAny<object[]>()), Times.Exactly(2));
		}

		private void RegisterType(Type abstraction, Type concrete, LifeCycle lifeCycle)
		{
			var testServiceItem = new TypeRegistryItem(abstraction, concrete, lifeCycle);
			_registryMock.Setup(x => x.GetRegistryItem(abstraction)).Returns(testServiceItem);
		}
	}
}
