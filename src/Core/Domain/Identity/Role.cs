namespace Domain.Identity;

using Primitives;
using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<Guid>, IAuditableEntity, IDeletableEntity
{
	public DateTime CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }

	public ICollection<UserRole> UserRoles { get; set; }
}