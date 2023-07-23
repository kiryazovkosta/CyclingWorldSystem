namespace Web.Models.Activities;

using Bikes;
using Common.Enumerations;

public class ActivityInputModel
{
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
    public string UserId { get; set; } = null!;
    public List<string> Pictures { get; set; } = new List<string>();
    public string? PicturesList { get; set; }
    public Guid GpxId { get; set; }
    
    public IEnumerable<SimpleBikeViewModel> Bikes = new HashSet<SimpleBikeViewModel>();
}
