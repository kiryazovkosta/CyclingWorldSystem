namespace Domain.Identity;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
}
