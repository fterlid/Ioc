namespace Fte.Ioc.Tests.Utils.TestServices.CircularDependencies
{
	public interface ISimpleCircularDepenceny
	{
	}

	public interface IOtherSimpleCircularDepenceny
	{
	}

	public class SimpleCircularDepenceny
	{
		public SimpleCircularDepenceny(IOtherSimpleCircularDepenceny otherSimple)
		{
		}
	}

	public class OtherSimpleCircularDepenceny
	{
		public OtherSimpleCircularDepenceny(ISimpleCircularDepenceny simple)
		{
		}
	}
}
