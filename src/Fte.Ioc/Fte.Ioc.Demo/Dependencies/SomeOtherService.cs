namespace Fte.Ioc.Demo.Dependencies
{
	public class SomeOtherService : ISomeOtherService
	{
		public SomeOtherService(ISomeService someService)
		{
		}
	}
}