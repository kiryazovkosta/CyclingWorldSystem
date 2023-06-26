

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ActivityLikeConfiguration : IEntityTypeConfiguration<ActivityLike>
{
	public void Configure(EntityTypeBuilder<ActivityLike> builder)
	{
		builder
			.ToTable(GlobalConstants.ActivityLike.TableName);

		builder
			.HasKey(al => new {al.UserId, al.ActivityId});

		builder
			.HasOne(al => al.User)
			.WithMany(u => u.Likes)
			.HasForeignKey(al => al.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(al => al.Activity)
			.WithMany(a => a.Likes)
			.HasForeignKey(al => al.ActivityId);
	}
}
