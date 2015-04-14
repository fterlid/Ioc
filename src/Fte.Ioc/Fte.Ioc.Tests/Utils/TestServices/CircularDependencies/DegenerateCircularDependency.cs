namespace Fte.Ioc.Tests.Utils.TestServices.CircularDependencies
{
	public interface IDegenerateCircularDependency
	{
	}

    public class DegenerateCircularDependency : IDegenerateCircularDependency
	{
		public DegenerateCircularDependency(IDegenerateCircularDependency otherDegenerate)
		{
		}
	}
}
