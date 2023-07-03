namespace Persistence;

using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : IdentityDbContext<User, Role, Guid,
	IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
	IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
	}

	public ApplicationDbContext()
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}
}