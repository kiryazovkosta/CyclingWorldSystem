using Web.Models.BikeTypes;

namespace Web.Models.Bikes;

public class BikeInputModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public decimal Weight { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string? Notes { get; set; }

    public string BikeTypeId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public IEnumerable<BikeTypeViewModel> BikeTypes { get; set; }
            = new HashSet<BikeTypeViewModel>();
}
