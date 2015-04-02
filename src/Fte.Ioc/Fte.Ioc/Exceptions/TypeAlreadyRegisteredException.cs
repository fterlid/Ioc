using System;

namespace Fte.Ioc.Exceptions
{
	public class TypeAlreadyRegisteredException : Exception
	{
		public TypeAlreadyRegisteredException()
		{
		}

		public TypeAlreadyRegisteredException(string message) : base(message)
		{
		}

		public TypeAlreadyRegisteredException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
