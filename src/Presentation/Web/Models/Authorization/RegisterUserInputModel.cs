namespace Web.Models.Authorization;

public class RegisterUserInputModel
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; } 
    public string LastName { get; set; } = null!;
    public IFormFile? Avatar { get; set; }
}
