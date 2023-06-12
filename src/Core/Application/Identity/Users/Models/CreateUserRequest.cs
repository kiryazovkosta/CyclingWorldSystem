namespace Application.Identity.Users.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed record CreateUserRequest(
	string UserName,
	string Email,
	string Password,
	string ConfirmPassword,
	string FirstName,
	string? MiddleName,
	string LastName);