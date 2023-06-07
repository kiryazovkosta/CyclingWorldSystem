namespace Domain.Exceptions.Base;

using System;

public abstract class BadRequestException : Exception
{
	protected BadRequestException(string message)
		: base(message)
	{
	}
}
