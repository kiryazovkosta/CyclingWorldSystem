using Domain.Primitives;

namespace Domain.Entities;

public class Workout : DeletableEntity
{
	public string Title { get; set; } = null!;

	public Guid TrainingPlanId { get; set; }
	public TrainingPlan TrainingPlan { get; set; } = null!;
}