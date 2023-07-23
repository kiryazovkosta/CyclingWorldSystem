namespace Web.Models.GpxFiles;

using Web.Models.Waypoints;

public class GpxFileViewModel
{
    public Guid GpxId { get; set; }
    public DateTime StartDateTime { get; set; }
    public double? Distance { get; set; }
    public decimal? PositiveElevation { get; set; }
    public decimal? NegativeElevation { get; set; }
    public TimeSpan Duration { get; set; }
}