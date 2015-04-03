using System;

namespace Fte.Ioc.Exceptions
{
	public class TypeCannotBeInstantiatedException : Exception
	{
		public TypeCannotBeInstantiatedException()
		{
		}

		public TypeCannotBeInstantiatedException(string message) : base(message)
		{
		}

		public TypeCannotBeInstantiatedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
