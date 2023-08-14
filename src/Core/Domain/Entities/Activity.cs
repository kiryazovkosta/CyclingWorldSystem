namespace Domain.Entities;

using Common.Constants;
using Common.Enumerations;
using Domain.Identity;
using Domain.Primitives;
using Errors;
using Shared;

public class Activity : DeletableEntity
{
    private Activity()
    {
		Waypoints = new HashSet<Waypoint>();
		Images = new HashSet<Image>();
		Likes = new HashSet<ActivityLike>();
		Comments = new HashSet<Comment>();
	}

    private Activity(string title, string description, string? privateNotes, decimal distance, 
	    TimeSpan duration, decimal? positiveElevation, decimal? negativeElevation,  
	    VisibilityLevelType visibilityLevel, DateTime startDateTime, Guid bikeId, Guid userId)
		: this()
    {
	    Title = title;
	    Description = description;
	    PrivateNotes = privateNotes;
	    Distance = distance;
	    Duration = duration;
	    PositiveElevation = positiveElevation;
	    NegativeElevation = negativeElevation;
	    VisibilityLevel = visibilityLevel;
	    StartDateTime = startDateTime;
	    BikeId = bikeId;
	    UserId = userId;
    }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PrivateNotes { get; set; }
    public decimal Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public decimal? PositiveElevation { get; set; }
    public decimal? NegativeElevation { get; set; }
    public VisibilityLevelType VisibilityLevel { get; set; }
    public DateTime StartDateTime { get; set; }
    public Guid BikeId { get; set; }
	public Bike Bike { get; set; } = null!;
	public Guid UserId { get; set; }
	public User User { get; set; } = null!;
	public ICollection<Waypoint> Waypoints { get; set; }
	public ICollection<Image> Images { get; private set; }
	public ICollection<ActivityLike> Likes { get; set; }
	public ICollection<Comment> Comments { get; set; }

	public static Result<Activity> Create(string title, string description, string? privateNotes, 
		decimal distance, TimeSpan duration, decimal? positiveElevation, decimal? negativeElevation, 
		VisibilityLevelType visibilityLevel, DateTime startDateTime, Guid bikeId, Guid userId)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			return Result.Failure<Activity>(DomainErrors.Activity.TitleIsNullOrEmpty);
		}
		
		if (title.Length < GlobalConstants.Activity.TitleMinLength ||
		    title.Length > GlobalConstants.Activity.TitleMaxLength)
		{
			return Result.Failure<Activity>(DomainErrors.Activity.InvalidTitleLength);
		}
		
		if (string.IsNullOrWhiteSpace(description))
		{
			return Result.Failure<Activity>(DomainErrors.Activity.DescriptionIsNullOrEmpty);
		}
		
		if (description.Length < GlobalConstants.Activity.DescriptionMinLength ||
		    description.Length > GlobalConstants.Activity.DescriptionMaxLength)
		{
			return Result.Failure<Activity>(DomainErrors.Activity.InvalidDescriptionLength);
		}
		
		var activity = new Activity(title, description, privateNotes, distance, duration, positiveElevation,
			negativeElevation, visibilityLevel, startDateTime, bikeId, userId);
		return activity;
	}

	public Result AddImages(List<string>? pictures)
	{
		if (pictures is null || !pictures.Any())
		{
			return Result.Success();
		}
		
		var images = new List<Image>();
		foreach (var picture in pictures)
		{
			images.Add(Image.Create(picture, this).Value);
		}

		this.Images = images;
		return Result.Success();
	}
}