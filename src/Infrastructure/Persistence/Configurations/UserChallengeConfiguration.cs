namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

public class UserChallengeConfiguration : IEntityTypeConfiguration<UserChallenge>
{
	public void Configure(EntityTypeBuilder<UserChallenge> builder)
	{
		builder
			.ToTable(GlobalConstants.UserChallenge.TableName);

		builder
			.HasIndex(uc => new { uc.UserId, uc.ChallengeId });

		builder
			.HasOne(uc => uc.User)
			.WithMany(u => u.Challenges)
			.HasForeignKey(u => u.UserId);
		builder 
			.HasOne(uc => uc.Challenge)
			.WithMany(c => c.Users)
			.HasForeignKey(uc => uc.ChallengeId);
	}
}