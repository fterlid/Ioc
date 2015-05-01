using System;
using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using Fte.Ioc.Tests.Utils.TestServices;
using NUnit.Framework;

namespace Fte.Ioc.Tests.Registry
{
	[TestFixture]
	public class TypeRegistryTest
	{
		private TypeRegistry _registry;

		[SetUp]
		public void TestInitialize()
		{
			_registry = new TypeRegistry();
		}

		[Test]
		public void Register_RegisterType_TypeIsRegistered()
		{
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);

			var registeredType = _registry.GetRegistryItem(typeof (ITestService));

			Assert.NotNull(registeredType);
			Assert.AreEqual(typeof(ITestService), registeredType.AbstractionType);
			Assert.AreEqual(typeof(TestService), registeredType.ConcreteType);
			Assert.AreEqual(LifeCycle.Singleton, registeredType.LifeCycle);
		}

		[Test]
		public void Discover_AssemblyAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				_registry.Discover<ITestService>(null, LifeCycle.Transient);
			});

			StringAssert.Contains("assembly", ex.Message);
		}

		[Test]
		public void Discover_ThisAssembly_RegistersDiscoveredServiceInstances()
		{
			var assembly = GetType().Assembly;
			_registry.Discover<IDiscoveredService>(assembly, LifeCycle.Transient);

			var regItem1 = _registry.GetRegistryItem(typeof (DiscoveredService));
			var regItem2 = _registry.GetRegistryItem(typeof (OtherDiscoveredService));

			Assert.IsNotNull(regItem1);
			Assert.IsNotNull(regItem2);
		}

		[Test]
		public void Discover_ThisAssembly_RegisteredItemsHasCorrectLifeCycle()
		{
			var assembly = GetType().Assembly;
			_registry.Discover<IDiscoveredService>(assembly, LifeCycle.Singleton);

			var regItem = _registry.GetRegistryItem(typeof (DiscoveredService));

			Assert.AreEqual(LifeCycle.Singleton, regItem.LifeCycle);
		}

		[Test]
		public void Register_RegisterTypeTwice_ThrowsException()
		{
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);

			Assert.Catch<TypeAlreadyRegisteredException>(() =>
			{
				_registry.Register<ITestService, TestService>(LifeCycle.Singleton);
			});
		}

		[Test]
		public void Register_RegisterInterfaceAsConcreteType_ThrowsException()
		{
			Assert.Catch<TypeCannotBeInstantiatedException>(() =>
			{
				_registry.Register<ITestService, ITestService>(LifeCycle.Singleton);
			});
		}

		[Test]
		public void GetRegistryItem_InputTypeIsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() =>
			{
				_registry.GetRegistryItem(null);
			});

			StringAssert.Contains("abstractionType", ex.Message);
		}

		[Test]
		public void GetRegistryItem_InputTypeIsNotRegistered_ThrowsException()
		{
			Assert.Catch<TypeNotRegisteredException>(() =>
			{
				_registry.GetRegistryItem(typeof(ITestService));
			});
		}

		[Test]
		public void GetRegistryItem_InputTypeIsRegistered_ReturnsItem()
		{
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);

			var item = _registry.GetRegistryItem(typeof(ITestService));

			Assert.IsNotNull(item);
		}
	}
}
