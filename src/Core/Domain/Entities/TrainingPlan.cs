using Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TrainingPlan : DeletableEntity
{
    public TrainingPlan()
    {
		Workouts = new HashSet<Workout>();
		Users = new HashSet<UserTrainingPlan>();
	}

    public string Title { get; set; } = null!;

	public ICollection<Workout> Workouts { get; set; }


	public ICollection<UserTrainingPlan> Users { get; set; }
}