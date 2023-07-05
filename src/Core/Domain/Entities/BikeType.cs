namespace Domain.Entities;

using Common.Constants;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

public class BikeType : DeletableEntity
{
    private BikeType()
    {
        this.Bikes = new HashSet<Bike>();
    }

    private BikeType(string name)
        : this()
    {
        this.Name = name;
    }

    public string Name { get; init; } = null!;

    public ICollection<Bike> Bikes { get; init; }

    public static Result<BikeType> Create(string name, bool exists)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<BikeType>(DomainErrors.BikeType.BikeTypeNameIsNull);
        }

        if (name.Length < GlobalConstants.BikeType.NameMinLength 
            || name.Length > GlobalConstants.BikeType.NameMaxLength) 
        {
			return Result.Failure<BikeType>(
                DomainErrors.BikeType.BikeTypeNameInvalidLength(
                    GlobalConstants.BikeType.NameMinLength, 
                    GlobalConstants.BikeType.NameMaxLength));
		}

        if (exists)
        {
			return Result.Failure<BikeType>(
	            DomainErrors.BikeType.BikeTypeNameExists(name));
		}

        var bikeType = new BikeType(name);
        return bikeType;
    }
}