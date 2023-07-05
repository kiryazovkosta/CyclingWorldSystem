namespace Application.Services;

using Application.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly UserManager<User> _userManager;

	public CurrentUserService(
		IHttpContextAccessor httpContextAccessor, 
		UserManager<User> userManager)
	{
		_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
	}

	public User? GetCurrentUser()
	{
		Guid currentUserId = GetCurrentUserId();
		return _userManager.Users.FirstOrDefault(u => u.Id == currentUserId);
	}

	public Guid GetCurrentUserId()
	{
		var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);


		if (id is string userIdString && Guid.TryParse(userIdString, out Guid userId))
		{
			return userId;
		}

		return Guid.Empty;
	}
}