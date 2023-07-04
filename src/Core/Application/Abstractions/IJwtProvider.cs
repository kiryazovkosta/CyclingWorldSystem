namespace Application.Abstractions;

using Domain.Identity;

public interface IJwtProvider
{
	string CreateToken(User user);
}