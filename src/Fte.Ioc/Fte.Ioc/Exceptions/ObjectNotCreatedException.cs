using System;

namespace Fte.Ioc.Exceptions
{
	public class ObjectNotCreatedException : Exception
	{
		public ObjectNotCreatedException()
		{
		}

		public ObjectNotCreatedException(string message) : base(message)
		{
		}

		public ObjectNotCreatedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
