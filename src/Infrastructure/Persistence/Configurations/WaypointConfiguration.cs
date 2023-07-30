namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WaypointConfiguration : IEntityTypeConfiguration<Waypoint>
{
	public void Configure(EntityTypeBuilder<Waypoint> builder)
	{
		// Table name
		builder
			.ToTable(GlobalConstants.Waypoint.TableName);

		// Primary key
		builder
			.HasKey(x => x.Id);

		// Properties
		builder
			.Property(w => w.Latitude)
			.HasPrecision(GlobalConstants.Waypoint.LatitudePrecision, GlobalConstants.Waypoint.LatitudeScale)
			.IsRequired();
		builder
			.Property(w => w.Longitude)
			.HasPrecision(GlobalConstants.Waypoint.LongitudePrecision, GlobalConstants.Waypoint.LongitudeScale)
			.IsRequired();
		builder
			.Property(w => w.Speed)
			.HasPrecision(GlobalConstants.Waypoint.SpeedPrecision, GlobalConstants.Waypoint.SpeedScale)
			.IsRequired();
		builder
			.Property(w => w.Elevation)
			.HasPrecision(GlobalConstants.Waypoint.SpeedPrecision, GlobalConstants.Waypoint.SpeedScale)
			.IsRequired();
		builder
			.Property(w => w.Temperature)
			.HasPrecision(GlobalConstants.Waypoint.SpeedPrecision, GlobalConstants.Waypoint.SpeedScale)
			.IsRequired();

		// Indexes
		builder
			.HasIndex(w => new { w.ActivityId, w.OrderIndex })
			.HasDatabaseName("UX_Waypoints_ActivityOrderIndex");

		// Relations
		builder
			.HasOne(w => w.Activity)
			.WithMany(a => a.Waypoints)
			.HasForeignKey(w => w.ActivityId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}