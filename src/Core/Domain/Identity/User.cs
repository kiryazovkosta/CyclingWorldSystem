namespace Domain.Identity;

using Domain.Entities;
using Domain.Primitives;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>, IAuditableEntity, IDeletableEntity
{
    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

	public string ImageUrl { get; set; } = null!;

	public ICollection<Activity> Activities { get; set; } = new HashSet<Activity>();

	public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

	public ICollection<ActivityLike> Likes { get; set; } = new HashSet<ActivityLike>();

	public ICollection<UserChallenge> Challenges { get; set; } = new HashSet<UserChallenge>();

	public ICollection<UserTrainingPlan> TrainingPlans { get; set; } = new HashSet<UserTrainingPlan>();

	public DateTime CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }

	public ICollection<UserRole> UserRoles { get; set; }
}
