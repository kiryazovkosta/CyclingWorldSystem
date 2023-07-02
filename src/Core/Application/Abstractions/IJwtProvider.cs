namespace Application.Abstractions;

using Domain.Identity;

public interface IJwtProvider
{
	string Generate(User user);
}