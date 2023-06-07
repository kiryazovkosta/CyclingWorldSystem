namespace Domain.Exceptions.Base;

using System;

public abstract class NotFoundException : Exception
{
	protected NotFoundException(string message)
		: base(message)
	{
	}
}