using Shared.Application.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Helpers;

namespace Shared.Application.Extensions;

/// <summary>
/// Extensions for IQueryable for working with table queries
/// </summary>
public static class TableExtensions
{
    /// <summary>
    /// Applies table filters to query
    /// </summary>
    /// <param name="source">query</param>
    /// <param name="filters">list of table filters</param>
    /// <typeparam name="TSource">entity</typeparam>
    /// <returns>query with applied filters</returns>
    public static IQueryable<TSource> Filters<TSource>(this IQueryable<TSource> source, IEnumerable<TableFilter>? filters)
        where TSource : class
    {
        if (filters != null)
        {
            var validFilters = filters
                .Where(p =>
                    !string.IsNullOrEmpty(p.Operator) &&
                    !string.IsNullOrEmpty(p.Value) &&
                    !(p.Operator == "in" && p.Value == "null"));

            if (validFilters != null && validFilters.Count() > 0)
                source = source.Where(ExpressionHelper.BuildPredicates<TSource>(validFilters));
        }

        return source;
    }

    /// <summary>
    /// Applies limiting field to array to a query
    /// </summary>
    /// <param name="source">Original query</param>
    /// <param name="Field">name of field to apply array contains to</param>
    /// <param name="array">array</param>
    /// <param name="UseEmptyAsAll">if true ignores empty array and returns original query, otherwise returns empty query</param>
    /// <typeparam name="TSource">entity</typeparam>
    /// <typeparam name="TRaion">type of array</typeparam>
    /// <returns>query with applied field in array</returns>
    public static IQueryable<TSource> FilterRaion<TSource, TRaion>(this IQueryable<TSource> source, string Field, IList<TRaion>? array, bool UseEmptyAsAll = true)
        where TSource : class
    {
        if (array != null && array.Count > 0)
            return source.Where(p => array.Contains(EF.Property<TRaion>(p, Field))); //this works
        else
        {
            if (UseEmptyAsAll)
                return source;
            else
                return source.Where(p => false);
        }
    }

    /// <summary>
    /// Applying table parameters to query (Ordering and Pagination)
    /// </summary>
    /// <param name="source">Original query</param>
    /// <param name="table">Table parameters</param>
    /// <param name="DefaultSort">Default sorting field name</param>
    /// <param name="DefaultSortDesc">Default sorting is descending (default is false)</param>
    /// <typeparam name="TSource">Entity</typeparam>
    /// <returns>Query with applied table parameters</returns>
    public static IQueryable<TSource> TableQuery<TSource>(this IQueryable<TSource> source, TableQuery table, string DefaultSort, bool DefaultSortDesc = false)
        where TSource : class
    {
        //Ordering
        if (string.IsNullOrEmpty(table.SortBy))
        {
            //default
            if (DefaultSortDesc)
                source = source.OrderByDescending(p => EF.Property<object>(p, DefaultSort));
            else
                source = source.OrderBy(p => EF.Property<object>(p, DefaultSort));
        }
        else
        {
            if (table.SortDesc)
                source = source.OrderByDescending(p => EF.Property<object>(p, table.SortBy));
            else
                source = source.OrderBy(p => EF.Property<object>(p, table.SortBy));
        }

        //Pagination
        if (table.Page == 1)
            //tagging is for fixing fetch first error in old DB version (9.8) with DB9 command interceptor
            return source.Take(table.ItemsPerPage).TagWith($"FIRST PAGE TABLE N{table.ItemsPerPage}");
        else
            return source.Skip((table.Page - 1) * table.ItemsPerPage).Take(table.ItemsPerPage);
    }
}
