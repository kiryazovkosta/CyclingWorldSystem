namespace Domain.Entities;

using Domain.Identity;

public class UserTrainingPlan
{
	public Guid UserId { get; set; }
	public User User { get; set; } = null!;

	public Guid TrainingPlanId { get; set; }
	public TrainingPlan TrainingPlan { get; set; } = null!;

	public bool IsComplited { get; set; }
}