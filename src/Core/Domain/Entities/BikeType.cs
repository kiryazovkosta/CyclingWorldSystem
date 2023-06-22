namespace Domain.Entities;

using Domain.Primitives;

public class BikeType : DeletetableEntity
{
    private BikeType()
    {
        this.Bikes = new HashSet<Bike>();
    }

    public string Name { get; init; } = null!;

    public ICollection<Bike> Bikes { get; init; }


}