using Fte.Ioc.Exceptions;
using Fte.Ioc.Registry;
using Fte.Ioc.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fte.Ioc.Tests.Registry
{
	[TestClass]
	public class TypeRegistryTest
	{
		[TestMethod]
		public void Register_RegisterType_Void()
		{
			var registry  = new TypeRegistry();

			registry.Register<ITestService, TestService>(LifeCycle.Singleton);
		}

		[TestMethod]
		[ExpectedException(typeof(TypeAlreadyRegisteredException))]
		public void Register_RegisterTypeTwice_ThrowsException()
		{
			var registry  = new TypeRegistry();

			registry.Register<ITestService, TestService>(LifeCycle.Singleton);
			registry.Register<ITestService, TestService>(LifeCycle.Singleton);
		}
	}
}
