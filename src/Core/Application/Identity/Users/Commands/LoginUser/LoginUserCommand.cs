namespace Application.Identity.Users.Commands.LoginUser;

using Application.Abstractions.Messaging;

public sealed record LoginUserCommand(string UserName, string Password) : ICommand<string>;