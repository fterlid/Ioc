using NUnit.Framework;

namespace Fte.Ioc.Tests
{
	[TestFixture]
	public class ContainerProviderTest
	{
		[Test]
		public void GetContainer_WhenCalled_ReturnsNonNullObject()
		{
			var container = ContainerProvider.GetContainer();
			Assert.IsNotNull(container);
		}
	}
}
