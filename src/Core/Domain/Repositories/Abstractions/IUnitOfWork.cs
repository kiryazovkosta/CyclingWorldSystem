namespace Domain.Repositories.Abstractions;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}