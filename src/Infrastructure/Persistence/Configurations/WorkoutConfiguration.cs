using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
	public void Configure(EntityTypeBuilder<Workout> builder)
	{
		builder.ToTable(GlobalConstants.Workout.TableName);

		builder.HasKey(w => w.Id);

		builder.Property(w => w.Title)
			.HasMaxLength(GlobalConstants.Workout.TitleMaxLength)
			.IsRequired();

		builder.HasOne(w => w.TrainingPlan)
			.WithMany(tp => tp.Workouts)
			.IsRequired()
			.HasForeignKey(w => w.TrainingPlanId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
