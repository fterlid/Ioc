using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using Fte.Ioc.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fte.Ioc.Tests.Registry
{
	[TestClass]
	public class TypeRegistryTest
	{
		private TypeRegistry _registry;

		[TestInitialize]
		public void TestInitialize()
		{
			_registry = new TypeRegistry();
		}

		[TestMethod]
		public void Register_RegisterType_Void()
		{
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);
		}

		[TestMethod]
		[ExpectedException(typeof(TypeAlreadyRegisteredException))]
		public void Register_RegisterTypeTwice_ThrowsException()
		{
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);
		}

		[TestMethod]
		[ExpectedException(typeof(TypeNotRegisteredException))]
		public void GetRegistryItem_InputTypeIsNotRegistered_ThrowsException()
		{
			_registry.GetRegistryItem(typeof(ITestService));
		}

		[TestMethod]
		public void GetRegistryItem_InputTypeIsRegistered_ReturnsItem()
		{
			_registry.Register<ITestService, TestService>(LifeCycle.Singleton);

			var item = _registry.GetRegistryItem(typeof(ITestService));

			Assert.IsNotNull(item);
		}
	}
}
