namespace Domain.Primitives;

public class DeletetableEntity : AuditableEntity, IDeletableEntity
{
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }
}