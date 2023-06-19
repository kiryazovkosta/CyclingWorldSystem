namespace Persistence.Interseptors;

using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class UpdateAuditableEntitiesInterseptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData, 
		InterceptionResult<int> result, 
		CancellationToken cancellationToken = default)
	{
		var dbContext = eventData.Context;
		if (dbContext is null)
		{
			return base.SavingChangesAsync(
				eventData,
				result,
				cancellationToken);
		}

		IEnumerable<EntityEntry<IAuditableEntity>> entries = 
			dbContext
				.ChangeTracker
				.Entries<IAuditableEntity>();
		foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
		{
			if (entityEntry.State == EntityState.Added)
			{
				entityEntry.Property(ee => ee.CreatedOn).CurrentValue = DateTime.UtcNow;
			}

			if (entityEntry.State == EntityState.Modified)
			{
				entityEntry.Property(ee => ee.ModifiedOn).CurrentValue = DateTime.UtcNow;
			}
		}

		return base.SavingChangesAsync(
			eventData, 
			result, 
			cancellationToken);
	}
}