// ------------------------------------------------------------------------------------------------
//  <copyright file="PagedList.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Infrastructure;

using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

public class PagedList<T, TId>
    where T : Entity
    where TId : IEquatable<TId>
{
    private readonly IEnumerable<T> _items;
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalItemsCount { get; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public IReadOnlyCollection<T> Items => _items.ToList().AsReadOnly();

    private PagedList(IEnumerable<T> records, int totalItemsCount, int pageNumber, int pageSize)
    {
        TotalItemsCount = totalItemsCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
        _items = records;
    }

    public static async Task<PagedList<T,TId>> ToPagedListAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalItemsCount = source.Count();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PagedList<T,TId>(items, totalItemsCount, pageNumber, pageSize);
    }
}