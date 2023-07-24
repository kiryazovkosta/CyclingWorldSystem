namespace Application.Identity.Users.Commands.UpdateUser;

using Abstractions.Messaging;

public sealed record UpdateUserCommand(
    Guid Id, 
    string UserName, 
    string Email,
    bool EmailConfirmed,
    string FirstName,
    string LastName,
    string? Password) : ICommand;