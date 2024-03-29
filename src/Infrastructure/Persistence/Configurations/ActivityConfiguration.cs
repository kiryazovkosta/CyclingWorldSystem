﻿namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
	public void Configure(EntityTypeBuilder<Activity> builder)
	{
		builder
			.ToTable(GlobalConstants.Activity.TableName);
		
		builder.HasQueryFilter(activity => !activity.IsDeleted);

		builder
			.HasKey(x => x.Id);

		builder
			.Property(a => a.Title)
			.HasMaxLength(GlobalConstants.Activity.TitleMaxLength)
			.IsRequired();
		builder
			.Property (a => a.Description)
			.HasMaxLength (GlobalConstants.Activity.DescriptionMaxLength)
			.IsRequired();
		builder
			.Property(a => a.PrivateNotes)
			.HasMaxLength(GlobalConstants.Activity.PrivateNotesMaxLength);
		builder
			.Property(a => a.Distance)
			.HasPrecision(GlobalConstants.Activity.DistancePrecision, GlobalConstants.Activity.DistanceScale)
			.IsRequired();
		builder
			.Property(a => a.PositiveElevation)
			.HasPrecision(GlobalConstants.Activity.ElevationPrecision, GlobalConstants.Activity.ElevationScale);
		builder
			.Property(a => a.NegativeElevation)
			.HasPrecision(GlobalConstants.Activity.ElevationPrecision, GlobalConstants.Activity.ElevationScale);

		builder.Property(a => a.StartDateTime)
			.HasDefaultValue(DateTime.UtcNow);

		builder
			.HasOne(a => a.Bike)
			.WithMany(b => b.Activities)
			.IsRequired()
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(a => a.Waypoints)
			.WithOne(w => w.Activity)
			.HasForeignKey(w => w.ActivityId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.HasMany(a => a.Images)
			.WithOne(i => i.Activity);

		builder
			.HasMany(a => a.Likes)
			.WithOne(al => al.Activity);

		builder
			.HasMany(a => a.Comments)
			.WithOne(c => c.Activity);
	}
}
