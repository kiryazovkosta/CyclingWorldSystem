namespace Domain.Entities;

using Common.Enumerations;
using Domain.Identity;
using Domain.Primitives;

public class Activity : DeletableEntity
{
    private Activity()
    {
		Waypoints = new HashSet<Waypoint>();
		Images = new HashSet<Image>();
		Likes = new HashSet<ActivityLike>();
		Comments = new HashSet<Comment>();
	}

    public string Title { get; set; } = null!;

	public string? Description { get; set; } = null!;

	public string? PrivateNotes { get; set; } = null!;

	public decimal Distance { get; set; }

	public TimeSpan Duration { get; set; }

	public int Elevation { get; set; }

	public VisibilityLevelType VisibilityLevel { get; set; }

	public Guid BikeId { get; set; }
	public Bike Bike { get; set; } = null!;

	public Guid UserId { get; set; }
	public User User { get; set; } = null!;

	public ICollection<Waypoint> Waypoints { get; set; }
	
	public ICollection<Image> Images { get; set; }

	public ICollection<ActivityLike> Likes { get; set; }

	public ICollection<Comment> Comments { get; set; }
}