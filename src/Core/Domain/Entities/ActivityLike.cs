namespace Domain.Entities;

using Domain.Identity;
using Domain.Primitives;

public class ActivityLike
{
	public Guid ActivityId { get; set; }
	public Activity Activity { get; set; } = null!;

	public Guid UserId { get; set; }
	public User User { get; set; } = null!;
}