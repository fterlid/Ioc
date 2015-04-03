using Fte.Ioc.Registry;
using Fte.Ioc.Resolver;
using Fte.Ioc.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fte.Ioc.Tests.Resolver
{
	[TestClass]
	public class ObjectFactoryTest
	{
		private IObjectFactory _factory;
		private TypeRegistryItem _singletonRegItem;
		private TypeRegistryItem _transientRegItem;

		[TestInitialize]
		public void TestInitialize()
		{
			_factory = new ObjectFactory();
			_singletonRegItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Singleton);
			_transientRegItem = new TypeRegistryItem(typeof(ITestService), typeof(TestService), LifeCycle.Transient);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_RegistryItemAsNull_ThrowsException()
		{
			_factory.Create(null, new object[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_ConstructorParamsArrayAsNull_ThrowsException()
		{
			_factory.Create(_transientRegItem, null);
		}

		[TestMethod]
		public void Create_ConcreteTypeAsInput_ReturnsObjectOfInputType()
		{
			var obj = _factory.Create(_transientRegItem, new object[0]);

			Assert.IsInstanceOfType(obj, typeof(TestService));
		}

		[TestMethod]
		public void Create_TypeHasSingletonLifeCycle_ObjectIsCreatedOnce()
		{
			var obj1 = _factory.Create(_singletonRegItem, new object[0]);
			var obj2 = _factory.Create(_singletonRegItem, new object[0]);

			Assert.AreSame(obj1, obj2);
		}

		[TestMethod]
		public void Resolve_TypeHasTransientLifeCycle_FactoryIsCalledOneachResolve()
		{
			var obj1 = _factory.Create(_transientRegItem, new object[0]);
			var obj2 = _factory.Create(_transientRegItem, new object[0]);

			Assert.AreNotSame(obj1, obj2);
		}
	}
}
