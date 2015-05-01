namespace Fte.Ioc.Tests.Utils.TestServices
{
	public class ComplexLegalDependency
	{
		public ComplexLegalDependency(ComplexLegalNodeA a, ComplexLegalNodeB b)
		{
		}
	}

	public class ComplexLegalNodeA
	{
	}

	public class ComplexLegalNodeB
	{
		public ComplexLegalNodeB(ComplexLegalNodeA a)
		{
		}
	}
}
