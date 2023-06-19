namespace Application.Identity.Users.Commands.CreateUser;

using Application.Abstractions.Messaging;
using Domain.Identity;
using System;

public sealed record CreateUserCommand(
	string UserName, 
	string Email, 
	string Password, 
	string ConfirmPassword, 
	string FirstName, 
	string? MiddleName, 
	string LastName) : ICommand<User>;