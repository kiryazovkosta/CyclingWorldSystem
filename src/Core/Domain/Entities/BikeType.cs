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

    public string Name { get; private set; } = null!;

    public ICollection<Bike> Bikes { get; init; }

    public static Result<BikeType> Create(string name, bool exists)
    {
		var nameValidationResult = ValidateName(name);
		if (!nameValidationResult.IsSuccess)
		{
			return Result.Failure<BikeType>(nameValidationResult.Error);
		}

		if (exists)
        {
			return Result.Failure<BikeType>(
	            DomainErrors.BikeType.BikeTypeNameExists(name));
		}

        var bikeType = new BikeType(name);
        return bikeType;
    }

	public Result Update(string name)
	{
		var nameValidationResult = ValidateName(name);
		if (!nameValidationResult.IsSuccess)
		{
			return Result.Failure<BikeType>(nameValidationResult.Error);
		}

		this.Name = name;
        return Result.Success();
	}

	private static Result ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return Result.Failure(DomainErrors.BikeType.BikeTypeNameIsNull);
		}

		if (name.Length < GlobalConstants.BikeType.NameMinLength
			|| name.Length > GlobalConstants.BikeType.NameMaxLength)
		{
			return Result.Failure(
				DomainErrors.BikeType.BikeTypeNameInvalidLength(
					GlobalConstants.BikeType.NameMinLength,
					GlobalConstants.BikeType.NameMaxLength));
		}

		return Result.Success();
	}
}