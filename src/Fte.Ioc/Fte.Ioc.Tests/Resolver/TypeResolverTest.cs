using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils.TestServices;
using Fte.Ioc.Tests.Utils.TestServices.CircularDependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Fte.Ioc.ObjectManagement;

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
		[ExpectedException(typeof(ArgumentNullException))]
		public void Resolve_TypeIsNull_ThrowsException()
		{
			_resolver.Resolve(null);
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

		[TestMethod]
		public void Resolve_TransientWithSingletonDependency_SingletonIsCreatedOnce()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			RegisterType(typeof(IOtherTestService), typeof(OtherTestService), LifeCycle.Transient);
			_objectManagerMock.Setup(x => x.HasInstance(
				It.Is<TypeRegistryItem>(y => y.LifeCycle == LifeCycle.Singleton)))
				.Returns(true);

			_resolver.Resolve(typeof(IOtherTestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Once);
		}

		[TestMethod]
		[Timeout(5000)] //Timeout if circular depenceny is not handled
		[ExpectedException(typeof(CircularDependencyException))]
		public void Resolve_TypeHasDegenerateCircularDependency_ThrowsException()
		{
			RegisterType(typeof(IDegenerateCircularDependency), typeof(DegenerateCircularDependency), LifeCycle.Transient);

			_resolver.Resolve(typeof(IDegenerateCircularDependency));
		}

		[TestMethod]
		[Timeout(5000)]	//Timeout if circular depenceny is not handled
		[ExpectedException(typeof(CircularDependencyException))]
		public void Resolve_TypeHasSimpleCircularDependency_ThrowsException()
		{
			RegisterType(typeof(ISimpleCircularDepenceny), typeof(SimpleCircularDepenceny), LifeCycle.Transient);
			RegisterType(typeof(IOtherSimpleCircularDepenceny), typeof(OtherSimpleCircularDepenceny), LifeCycle.Transient);

			_resolver.Resolve(typeof(ISimpleCircularDepenceny));
		}

		[TestMethod]
		[Timeout(5000)]	//Timeout if circular depenceny is not handled
		[ExpectedException(typeof(CircularDependencyException))]
		public void Resolve_TypeHasComplexCircularDependency_ThrowsException()
		{
			RegisterType(typeof(IComplexCircularDependency), typeof(ComplexCircularDependency), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyA), typeof(ComplexCircularDependencyA), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyB), typeof(ComplexCircularDependencyB), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyC), typeof(ComplexCircularDependencyC), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyD), typeof(ComplexCircularDependencyD), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyE), typeof(ComplexCircularDependencyE), LifeCycle.Transient);

			_resolver.Resolve(typeof(IComplexCircularDependency));
		}

		private void RegisterType(Type abstraction, Type concrete, LifeCycle lifeCycle)
		{
			var testServiceItem = new TypeRegistryItem(abstraction, concrete, lifeCycle);
			_registryMock.Setup(x => x.GetRegistryItem(abstraction)).Returns(testServiceItem);
		}
	}
}
