namespace Domain.Primitives;

public class AuditableEntity : Entity, IAuditableEntity
{
	public DateTime CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
}
