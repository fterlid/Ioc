namespace Fte.Ioc.Tests.Utils.TestServices.CircularDependencies
{
	public interface IComplexCircularDependency
	{
	}

	public interface IComplexCircularDependencyA
	{
	}

	public interface IComplexCircularDependencyB
	{
	}

	public interface IComplexCircularDependencyC
	{
	}

	public interface IComplexCircularDependencyD
	{
	}

	public interface IComplexCircularDependencyE
	{
	}

	public class ComplexCircularDependency
	{
		public ComplexCircularDependency(IComplexCircularDependencyA a, IComplexCircularDependencyB b)
		{
		}
	}

	public class ComplexCircularDependencyA
	{
		public ComplexCircularDependencyA(IComplexCircularDependencyC c)
		{
		}
	}

	public class ComplexCircularDependencyB
	{
		public ComplexCircularDependencyB(IComplexCircularDependencyC c)
		{
		}
	}

	public class ComplexCircularDependencyC
	{
		public ComplexCircularDependencyC(IComplexCircularDependencyD d)
		{
		}
	}

	public class ComplexCircularDependencyD
	{
		public ComplexCircularDependencyD(IComplexCircularDependencyB b, IComplexCircularDependencyE e)
		{
		}
	}

	public class ComplexCircularDependencyE
	{
	}
}
