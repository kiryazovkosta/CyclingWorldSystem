namespace Application.Identity.Users.Models;

using Microsoft.AspNetCore.Http;

public sealed record CreateUserRequest(
	string UserName,
	string Email,
	string Password,
	string ConfirmPassword,
	string FirstName,
	string? MiddleName,
	string LastName,
	IFormFile? Avatar);