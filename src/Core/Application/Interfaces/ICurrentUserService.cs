namespace Application.Interfaces;

using Domain.Identity;
using System;

public interface ICurrentUserService
{
	Guid GetCurrentUserId();

	User? GetCurrentUser();
}