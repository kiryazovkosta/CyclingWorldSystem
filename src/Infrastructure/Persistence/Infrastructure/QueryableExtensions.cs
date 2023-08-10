// ------------------------------------------------------------------------------------------------
//  <copyright file="QueryableExtensions.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Infrastructure;

using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Domain.Primitives;

public static class QueryableExtensions
{
    public static async Task<IPagedList<T, TId>> ToPagedListAsync<T, TId>
    (
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    )
        where T : Entity
        where TId : IEquatable<TId>
    {
        return await PagedList<T, TId>.ToPagedListAsync(source,
            pageNumber,
            pageSize,
            cancellationToken);
    }
    
    public static IQueryable<T> Sort<T>(
        this IQueryable<T> source,
        string? orderByQueryString,
        string? defaultSorting)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return string.IsNullOrWhiteSpace(defaultSorting)
                ? source
                : source.OrderBy(defaultSorting);
        }

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var orderQueryBuilder = new StringBuilder();
        var filteredParams = orderParams.Where(x => !string.IsNullOrWhiteSpace(x));

        foreach (var param in filteredParams)
        {
            var propertyFromQueryName = param.Trim().Split(" ")[0];
            var objectProperty = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(
                    propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
            {
                continue;
            }

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return string.IsNullOrWhiteSpace(defaultSorting)
                ? source
                : source.OrderBy<T>(defaultSorting);
        }

        return source.OrderBy(orderQuery);
    }
    
    public static IQueryable<T> Filter<T>(
        this IQueryable<T> source,
        string? filterByQueryString)
    {
        if (string.IsNullOrWhiteSpace(filterByQueryString))
        {
            return source;
        }

        var filterParams = filterByQueryString.Trim().Split(';');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var filterQueryBuilder = new List<string>();
        var filteredParams = filterParams.Where(x => !string.IsNullOrWhiteSpace(x));

        foreach (var param in filteredParams)
        {
            var propertyFromQuery = param.Trim().Split(":");
            if (propertyFromQuery.Length != 2)
            {
                continue;
            }
            
            var propertyFromQueryName = propertyFromQuery[0];
            var propertyFromQueryValue = propertyFromQuery[1];
            var objectProperty = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(
                    propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
            {
                continue;
            }

            filterQueryBuilder.Add($"{objectProperty.Name}.Contains(\"{propertyFromQueryValue}\")");
        }

        var filterQuery = string.Join(" && ", filterQueryBuilder).TrimEnd();
        return string.IsNullOrWhiteSpace(filterQuery) ? 
            source 
            : source.Where(filterQuery);
    }
}