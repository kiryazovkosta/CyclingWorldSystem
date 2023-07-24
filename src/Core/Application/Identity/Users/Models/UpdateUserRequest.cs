namespace Application.Identity.Users.Models;

public sealed record UpdateUserRequest(
    Guid Id, 
    string UserName, 
    string Email,
    bool EmailConfirmed,
    string FirstName,
    string LastName,
    string? Password);