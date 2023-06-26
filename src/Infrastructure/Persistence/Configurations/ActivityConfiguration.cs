namespace Persistence.Configurations;

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
			.HasPrecision(GlobalConstants.Activity.DestancePrecision, GlobalConstants.Activity.DestanceScale)
			.IsRequired();

		builder
			.HasOne(a => a.Bike)
			.WithMany(b => b.Activities)
			.IsRequired()
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(a => a.Waypoints)
			.WithOne(w => w.Activity)
			.HasForeignKey(w => w.ActivityId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Restrict);

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
