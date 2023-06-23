namespace Domain.Primitives;

public class DeletableEntity : AuditableEntity, IDeletableEntity
{
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }
}