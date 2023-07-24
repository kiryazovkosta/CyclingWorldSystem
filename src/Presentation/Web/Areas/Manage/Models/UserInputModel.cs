namespace Web.Areas.Manage.Models;

public class UserInputModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Password { get; set; }
}