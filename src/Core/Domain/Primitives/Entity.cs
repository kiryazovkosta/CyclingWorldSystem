namespace Domain.Primitives;

using System;

public abstract class Entity : IEquatable<Entity>
{
	protected Entity(Guid id)
	{
		Id = id;
	}

	protected Entity() 
		: this(Guid.NewGuid()) 
	{ }

	public Guid Id { get; init; }

	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}

        if (obj.GetType() != GetType())
        {
			return false;
        }

		if (obj is not Entity entity)
		{
			return false;
		}

		return entity.Id == Id;
    }

	public override int GetHashCode()
	{
		return Id.GetHashCode() * 41;
	}

	public bool Equals(Entity? other)
	{
		if (other is null)
		{
			return false;
		}

		if (other.GetType() != GetType())
		{
			return false;
		}

		return other.Id == Id;
	}
}
