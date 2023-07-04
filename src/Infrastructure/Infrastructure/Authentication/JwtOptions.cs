namespace Infrastructure.Authentication;

public class JwtOptions
{
    public int ExpirationMinutes { get; init; }
	public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
}
