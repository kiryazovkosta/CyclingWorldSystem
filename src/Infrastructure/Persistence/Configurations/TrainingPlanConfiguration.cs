namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TrainingPlanConfiguration : IEntityTypeConfiguration<TrainingPlan>
{
	public void Configure(EntityTypeBuilder<TrainingPlan> builder)
	{
		builder
			.ToTable("TrainingPlans");

		builder
			.HasKey(tp => tp.Id);

		builder
			.Property(tp => tp.Title)
			.HasMaxLength(GlobalConstants.TrainingPlan.TitleMaxLength)
			.IsRequired();

		builder
			.HasMany(tp => tp.Workouts)
			.WithOne(w => w.TrainingPlan)
			.HasForeignKey(w => w.TrainingPlanId);
	}
}