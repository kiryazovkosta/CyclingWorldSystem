namespace Web.Areas.Manage.Models;

public sealed record UserViewModel(
    Guid Id, 
    string UserName, 
    string Email, 
    bool EmailConfirmed,
    string FirstName,
    string LastName,
    string ImageUrl,
    List<string> Roles);