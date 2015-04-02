using System;

namespace Fte.Ioc.Exceptions
{
	public class TypeNotRegisteredException : Exception
	{
		public TypeNotRegisteredException()
		{
		}

		public TypeNotRegisteredException(string message) : base(message)
		{
		}

		public TypeNotRegisteredException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
