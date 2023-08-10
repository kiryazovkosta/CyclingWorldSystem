namespace Domain.Primitives;

public interface IPagedList<T, TId>
    where T: Entity
    where TId : IEquatable<TId>
{
    int CurrentPage { get; }
    int TotalPages { get; }
    int PageSize { get; }
    int TotalItemsCount { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }

    IReadOnlyCollection<T> Items { get; }
}