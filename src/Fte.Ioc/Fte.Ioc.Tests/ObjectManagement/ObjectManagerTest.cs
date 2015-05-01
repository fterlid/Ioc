using System;
using Fte.Ioc.Exceptions;
using Fte.Ioc.ObjectManagement;
using Fte.Ioc.Registry;
using Fte.Ioc.Tests.Utils.TestServices;
using NUnit.Framework;

namespace Fte.Ioc.Tests.ObjectManagement
{
	[TestFixture]
	public class ObjectManagerTest
	{
		private IObjectManager _manager;
		private TypeRegistryItem _singletonRegItem;
		private TypeRegistryItem _transientRegItem;

		[SetUp]
		public void TestInitialize()
		{
			_manager = new ObjectManager();
			_singletonRegItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			_transientRegItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Transient);
		}

		[Test]
		public void Create_RegistryItemAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() => _manager.Create(null, new object[0]));
			StringAssert.Contains("registryItem", ex.Message);
		}

		[Test]
		public void Create_ConstructorParamsArrayAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() => _manager.Create(_transientRegItem, null));
			StringAssert.Contains("constructorParams", ex.Message);
		}

		[Test]
		public void Create_ConcreteTypeAsInput_ReturnsObjectOfInputType()
		{
			var obj = _manager.Create(_transientRegItem, new object[0]);

			Assert.IsInstanceOf<TestService>(obj);
		}

		[Test]
		public void Create_TypeHasSingletonLifeCycle_ObjectIsCreatedOnce()
		{
			var obj1 = _manager.Create(_singletonRegItem, new object[0]);
			var obj2 = _manager.Create(_singletonRegItem, new object[0]);

			Assert.IsNotNull(obj1);
			Assert.AreSame(obj1, obj2);
		}

		[Test]
		public void Create_TypeHasTransientLifeCycle_ObjectIsCreatedOnEachCall()
		{
			var obj1 = _manager.Create(_transientRegItem, new object[0]);
			var obj2 = _manager.Create(_transientRegItem, new object[0]);

			Assert.IsNotNull(obj1);
			Assert.AreNotSame(obj1, obj2);
		}

		[Test]
		public void HasInstance_RegistryItemAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() => _manager.HasInstance(null));
			StringAssert.Contains("registryItem", ex.Message);
		}

		[Test]
		public void HasInstance_TypeHasSingletonLifeCycle_FalseBeforeCreateHasBeenCalled()
		{
			var result = _manager.HasInstance(_singletonRegItem);

			Assert.IsFalse(result);
		}

		[Test]
		public void HasInstance_TypeHasSingletonLifeCycle_TrueAfterCreateHasBeenCalled()
		{
			_manager.Create(_singletonRegItem, new object[0]);

			var result = _manager.HasInstance(_singletonRegItem);

			Assert.IsTrue(result);
		}

		[Test]
		public void HasInstance_TypeHasTransientLifeCycle_False()
		{
			_manager.Create(_transientRegItem, new object[0]);

			var result = _manager.HasInstance(_transientRegItem);

			Assert.IsFalse(result);
		}

		[Test]
		public void GetInstance_RegistryItemAsNull_ThrowsException()
		{
			var ex = Assert.Catch<ArgumentNullException>(() => _manager.GetInstance(null));
			StringAssert.Contains("registryItem", ex.Message);
		}

		[Test]
		public void GetInstance_SingletonHasBeenCreated_AlwaysReturnsSameObject()
		{
			_manager.Create(_singletonRegItem, new object[0]);

			var obj1 = _manager.GetInstance(_singletonRegItem);
			var obj2 = _manager.GetInstance(_singletonRegItem);

			Assert.IsNotNull(obj1);
			Assert.AreSame(obj1, obj2);
		}

		[Test]
		public void GetInstance_SingletonHasNotBeenCreated_ThrowsException()
		{
			Assert.Catch<ObjectNotCreatedException>(() => _manager.GetInstance(_singletonRegItem));
		}
	}
}
