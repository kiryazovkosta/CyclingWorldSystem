namespace Web.Models.Authorization;

public sealed record LogInResponse(string UserName, string Email, string Token);

