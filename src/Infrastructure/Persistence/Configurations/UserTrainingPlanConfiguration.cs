namespace Persistence.Configurations;

using Common.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserTrainingPlanConfiguration : IEntityTypeConfiguration<UserTrainingPlan>
{
	public void Configure(EntityTypeBuilder<UserTrainingPlan> builder)
	{
		builder
			.ToTable(GlobalConstants.UserTrainingPlan.TableName);

		builder
			.HasKey(utp => new { utp.UserId, utp.TrainingPlanId });

		builder
			.Property(utp => utp.IsComplited)
			.IsRequired();

		builder
			.HasOne(utp => utp.User)
			.WithMany(u => u.TrainingPlans)
			.HasForeignKey(utp => utp.UserId);
		builder
			.HasOne(utp => utp.TrainingPlan)
			.WithMany(tp => tp.Users)
			.HasForeignKey(utp => utp.TrainingPlanId);
	}
}