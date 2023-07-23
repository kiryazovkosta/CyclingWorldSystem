namespace Domain.Identity;

using Microsoft.AspNetCore.Identity;
using System;

public class UserRole : IdentityUserRole<Guid>
{
	public virtual User User { get; set; } = null!;

	public virtual Role Role { get; set; } = null!;
}