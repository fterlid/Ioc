using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fte.Ioc.Tests.Resolver
{
	[TestClass]
	public class ObjectManagerTest
	{
		private IObjectManager _manager;
		private TypeRegistryItem _singletonRegItem;
		private TypeRegistryItem _transientRegItem;

		[TestInitialize]
		public void TestInitialize()
		{
			_manager = new ObjectManager();
			_singletonRegItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			_transientRegItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Transient);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_RegistryItemAsNull_ThrowsException()
		{
			_manager.Create(null, new object[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_ConstructorParamsArrayAsNull_ThrowsException()
		{
			_manager.Create(_transientRegItem, null);
		}

		[TestMethod]
		public void Create_ConcreteTypeAsInput_ReturnsObjectOfInputType()
		{
			var obj = _manager.Create(_transientRegItem, new object[0]);

			Assert.IsInstanceOfType(obj, typeof(TestService));
		}

		[TestMethod]
		public void Create_TypeHasSingletonLifeCycle_ObjectIsCreatedOnce()
		{
			var obj1 = _manager.Create(_singletonRegItem, new object[0]);
			var obj2 = _manager.Create(_singletonRegItem, new object[0]);

			Assert.IsNotNull(obj1);
			Assert.AreSame(obj1, obj2);
		}

		[TestMethod]
		public void Create_TypeHasTransientLifeCycle_ObjectIsCreatedOnEachCall()
		{
			var obj1 = _manager.Create(_transientRegItem, new object[0]);
			var obj2 = _manager.Create(_transientRegItem, new object[0]);

			Assert.IsNotNull(obj1);
			Assert.AreNotSame(obj1, obj2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HasInstance_RegistryItemAsNull_ThrowsException()
		{
			_manager.HasInstance(null);
		}

		[TestMethod]
		public void HasInstance_TypeHasSingletonLifeCycle_FalseBeforeCreateHasBeenCalled()
		{
			var result = _manager.HasInstance(_singletonRegItem);
			Assert.IsFalse(result, "HasInstance should return false if object has not yet been created.");
		}

		[TestMethod]
		public void HasInstance_TypeHasSingletonLifeCycle_TrueAfterCreateHasBeenCalled()
		{
			_manager.Create(_singletonRegItem, new object[0]);
			var result = _manager.HasInstance(_singletonRegItem);
			Assert.IsTrue(result, "HasInstance should return true if object has been created.");
		}

		[TestMethod]
		public void HasInstance_TypeHasTransientLifeCycle_False()
		{
			_manager.Create(_transientRegItem, new object[0]);
			var result = _manager.HasInstance(_transientRegItem);
			Assert.IsFalse(result, "HasInstance should return false if object is not registered as singleton.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetInstance_RegistryItemAsNull_ThrowsException()
		{
			_manager.GetInstance(null);
		}

		[TestMethod]
		public void GetInstance_SingletonHasBeenCreated_AlwaysReturnsSameObject()
		{
			_manager.Create(_singletonRegItem, new object[0]);

			var obj1 = _manager.GetInstance(_singletonRegItem);
			var obj2 = _manager.GetInstance(_singletonRegItem);

			Assert.IsNotNull(obj1);
			Assert.AreSame(obj1, obj2);
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectNotCreatedException))]
		public void GetInstance_SingletonHasNotBeenCreated_ThrowsException()
		{
			_manager.GetInstance(_singletonRegItem);
		}
	}
}
