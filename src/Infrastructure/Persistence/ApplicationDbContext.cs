namespace Persistence;

using Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
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
