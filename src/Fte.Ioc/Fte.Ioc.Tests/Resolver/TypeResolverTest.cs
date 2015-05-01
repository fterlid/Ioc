using System;
using Fte.Ioc.Exceptions;
using Fte.Ioc.ObjectManagement;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils.TestServices;
using Fte.Ioc.Tests.Utils.TestServices.CircularDependencies;
using Moq;
using NUnit.Framework;

namespace Fte.Ioc.Tests.Resolver
{
	[TestFixture]
	public class TypeResolverTest
	{
		private Mock<ITypeRegistry> _registryMock;
		private Mock<IObjectManager> _objectManagerMock;
		private ITypeResolver _resolver;

		[SetUp]
		public void Setup()
		{
			_registryMock = new Mock<ITypeRegistry>();
			_objectManagerMock = new Mock<IObjectManager>();
			_resolver = new TypeResolver(_registryMock.Object, _objectManagerMock.Object);
		}

		[Test]
		public void Ctor_TypeRegistryAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				new TypeResolver(null, _objectManagerMock.Object);
			});

			StringAssert.Contains("typeRegistry", ex.Message);
		}

		[Test]
		public void Ctor_ObjectManagerAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				new TypeResolver(_registryMock.Object, null);
			});

			StringAssert.Contains("objectManager", ex.Message);
		}

		[Test]
		public void Resolve_TypeIsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				_resolver.Resolve(null);
			});

			StringAssert.Contains("typeToResolve", ex.Message);
		}

		[Test]
		public void Resolve_TypeWithDefaultConstructorIsRegistered_ObjectIsResolved()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(ITestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Once);
		}

		[Test]
		public void Resolve_TypeWithRegisteredDependencyIsRegistered_ObjectIsResolved()
		{
			RegisterType(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			RegisterType(typeof(IOtherTestService), typeof(OtherTestService), LifeCycle.Singleton);

			_resolver.Resolve(typeof(IOtherTestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Exactly(2));
		}

		[Test]
		public void Resolve_SingeltonWithTransientDependency_NoObjectsAreCreatedIfSingletonHasBeenCreated()
		{
			RegisterType(typeof(IOtherTestService), typeof(OtherTestService), LifeCycle.Singleton);
			_objectManagerMock.Setup(x => x.HasInstance(It.IsAny<TypeRegistryItem>())).Returns(true);

			_resolver.Resolve(typeof(IOtherTestService));

			_objectManagerMock.Verify(x => x.Create(It.IsAny<TypeRegistryItem>(), It.IsAny<object[]>()), Times.Never);
		}

		[Test]
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

		[Test, Timeout(3000)] //Timeout if circular depenceny is not handled
		public void Resolve_TypeHasDegenerateCircularDependency_ThrowsException()
		{
			RegisterType(typeof(IDegenerateCircularDependency), typeof(DegenerateCircularDependency), LifeCycle.Transient);

			Assert.Catch<CircularDependencyException>(() =>
			{
				_resolver.Resolve(typeof(IDegenerateCircularDependency));
			});
		}

		[Test, Timeout(3000)] //Timeout if circular depenceny is not handled
		public void Resolve_TypeHasSimpleCircularDependency_ThrowsException()
		{
			RegisterType(typeof(ISimpleCircularDepenceny), typeof(SimpleCircularDepenceny), LifeCycle.Transient);
			RegisterType(typeof(IOtherSimpleCircularDepenceny), typeof(OtherSimpleCircularDepenceny), LifeCycle.Transient);

			Assert.Catch<CircularDependencyException>(() =>
			{
				_resolver.Resolve(typeof(ISimpleCircularDepenceny));
			});
		}

		[Test, Timeout(3000)] //Timeout if circular depenceny is not handled
		public void Resolve_TypeHasComplexCircularDependency_ThrowsException()
		{
			RegisterType(typeof(IComplexCircularDependency), typeof(ComplexCircularDependency), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyA), typeof(ComplexCircularDependencyA), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyB), typeof(ComplexCircularDependencyB), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyC), typeof(ComplexCircularDependencyC), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyD), typeof(ComplexCircularDependencyD), LifeCycle.Transient);
			RegisterType(typeof(IComplexCircularDependencyE), typeof(ComplexCircularDependencyE), LifeCycle.Transient);

			Assert.Catch<CircularDependencyException>(() =>
			{
				_resolver.Resolve(typeof(IComplexCircularDependency));
			});
		}

		[Test]
		public void Resolve_MultipleTypesShareDependency_ResolvesObject()
		{
			RegisterType(typeof(ComplexLegalDependency), typeof(ComplexLegalDependency), LifeCycle.Transient);
			RegisterType(typeof(ComplexLegalNodeA), typeof(ComplexLegalNodeA), LifeCycle.Transient);
			RegisterType(typeof(ComplexLegalNodeB), typeof(ComplexLegalNodeB), LifeCycle.Transient);

			_resolver.Resolve(typeof (ComplexLegalDependency));

			_objectManagerMock.Verify(x => x.Create(
				It.Is<TypeRegistryItem>(item => item.AbstractionType == typeof(ComplexLegalDependency)), 
				It.IsAny<object[]>()), 
				Times.Once);
		}

		private void RegisterType(Type abstraction, Type concrete, LifeCycle lifeCycle)
		{
			var testServiceItem = new TypeRegistryItem(abstraction, concrete, lifeCycle);
			_registryMock.Setup(x => x.GetRegistryItem(abstraction)).Returns(testServiceItem);
		}
	}
}
