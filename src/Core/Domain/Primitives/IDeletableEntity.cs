namespace Domain.Primitives;

public interface IDeletableEntity
{
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }
}