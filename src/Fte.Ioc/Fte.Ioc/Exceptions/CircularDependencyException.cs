﻿using System;

namespace Fte.Ioc.Exceptions
{
	public class CircularDependencyException : Exception
	{
		public CircularDependencyException()
		{
		}

		public CircularDependencyException(string message) : base(message)
		{
		}

		public CircularDependencyException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
