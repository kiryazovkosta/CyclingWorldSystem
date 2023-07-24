namespace Application.Identity.Users.Commands.DeleteUser;

using Abstractions.Messaging;

public sealed record DeleteUserCommand(Guid Id) : ICommand;