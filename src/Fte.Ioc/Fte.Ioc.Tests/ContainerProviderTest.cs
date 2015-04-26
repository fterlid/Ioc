using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fte.Ioc.Tests
{
	[TestClass]
	public class ContainerProviderTest
	{
		[TestMethod]
		public void GetContainer_MethodIsCalled_ReturnsNonNullObject()
		{
			var container = ContainerProvider.GetContainer();
			Assert.IsNotNull(container);
		}
	}
}
