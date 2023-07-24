namespace Domain.Identity;

using Common.Constants;
using Domain.Entities;
using Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Shared;

public class User : IdentityUser<Guid>, IAuditableEntity, IDeletableEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

	public string ImageUrl { get; set; } = GlobalConstants.Cloudinary.DefaultAvatar;

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

	public string FullName => $"{this.FirstName} {this.LastName}";

	public Result Update(string userName, string email, bool isConfirmed, string firstName, string lastName)
	{
		this.UserName = userName;
		this.Email = email;
		this.EmailConfirmed = isConfirmed;
		this.FirstName = firstName;
		this.LastName = lastName;
		return Result.Success();
	}
}
