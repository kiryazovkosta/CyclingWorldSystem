namespace Domain.Entities;

using Domain.Identity;
using Domain.Primitives;

public class Comment : DeletableEntity
{
	public string Content { get; set; } = null!;

	public Guid UserId { get; set; }
	public User User { get; set; } = null!;

	public Guid ActivityId { get; set; }
	public Activity Activity { get; set; } = null!;
}