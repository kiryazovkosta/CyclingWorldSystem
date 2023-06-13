namespace Persistence.Configurations;

using Common.Constants;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(user => user.FirstName)
			.HasMaxLength(GlobalConstants.User.FirstNameMaxLength)
			.IsRequired();
		builder.Property(user => user.LastName)
			.HasMaxLength(GlobalConstants.User.LastNameMaxLength)
			.IsRequired();
	}
}