using Domain.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interseptors;

public class UpdateDeletableEntitiesInterseptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		var dbContext = eventData.Context;
		if (dbContext is null)
		{
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		IEnumerable<EntityEntry<IDeletableEntity>> entries =
			dbContext
				.ChangeTracker
				.Entries<IDeletableEntity>();
		foreach (EntityEntry<IDeletableEntity> entityEntry in entries)
		{
			if (entityEntry.State == EntityState.Deleted)
			{
				entityEntry.State = EntityState.Modified;
				entityEntry.Property(ee => ee.DeletedOn).CurrentValue = DateTime.UtcNow;
				entityEntry.Property(ee => ee.IsDeleted).CurrentValue = true;
			}
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}