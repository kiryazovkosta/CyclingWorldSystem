using Common.Constants;
using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class UserNotFoundException : NotFoundException
{
	public UserNotFoundException(string userName)
		: base(string.Format(GlobalConstants.User.NotFoundMessage, userName))
	{
	}
}