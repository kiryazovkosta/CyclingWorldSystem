namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChallengeConfiguration : IEntityTypeConfiguration<Challenge>
{
	public void Configure(EntityTypeBuilder<Challenge> builder)
	{
		builder
			.ToTable(GlobalConstants.Challenge.TableName);

		builder
			.HasKey(c => c.Id);

		builder
			.Property(c => c.Title)
			.HasMaxLength(GlobalConstants.Challenge.TitleMaxLength)
			.IsRequired();

		builder
			.Property<string>(c => c.Description)
			.HasMaxLength (GlobalConstants.Challenge.DescriptionMaxLength)
			.IsRequired();
	}
}
