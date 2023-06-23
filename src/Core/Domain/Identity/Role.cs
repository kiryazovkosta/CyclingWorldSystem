namespace Domain.Identity;

using Domain.Primitives;
using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<Guid>, IAuditableEntity, IDeletableEntity
{
	public DateTime CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }
}