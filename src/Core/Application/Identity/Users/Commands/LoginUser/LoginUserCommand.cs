namespace Application.Identity.Users.Commands.LogInUser;

using Application.Abstractions.Messaging;
using Application.Identity.Users.Models;

public sealed record LogInUserCommand(string UserName, string Password) : ICommand<LogInUserResponse>;