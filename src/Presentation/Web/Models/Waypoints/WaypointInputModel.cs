namespace Web.Models.Waypoints;

public class WaypointInputModel
{
    public int OrderNumber { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public decimal? Elevation { get; set; }

    public DateTime Time { get; set; }

    public decimal? Temperature { get; set; }

    public byte? HeartRate { get; set; }

    public decimal? Speed { get; set; }

    public ushort? Power { get; set; }

    public byte? Cadance { get; set; }
}
