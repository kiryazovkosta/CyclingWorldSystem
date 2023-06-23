namespace Domain.Entities;

using Domain.Identity;
using System;

public class UserChallenge
{
	public Guid UserId { get; set; }
	public User User { get; set; } = null!;

	public Guid ChallengeId { get; set; }
	public Challenge Challenge { get; set; } = null!;
}