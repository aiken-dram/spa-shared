using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Helpers;

namespace Shared.Application.Extensions;

/// <summary>
/// Extensions for IQueryable
/// </summary>
public static class IQueryableExtensions
{
    private sealed class holdPropertyValue<T>
    {
        public T? v;
    }

    /// <summary>
    /// Not sure where i used it, yare yare
    /// </summary>
    /// <param name="query"></param>
    /// <param name="propertyName"></param>
    /// <param name="propertyValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> WhereEquals<T, TValue>(this IQueryable<T> query, string propertyName, TValue propertyValue)
    {
        // p
        var pe = Expression.Parameter(typeof(T), "p");

        // p.{propertyName}
        var property = Expression.PropertyOrField(pe, propertyName);
        var holdpv = new holdPropertyValue<TValue> { v = propertyValue };
        // holdpv.v
        var value = Expression.PropertyOrField(Expression.Constant(holdpv), "v");

        // p.{propertyName} == holdpv.v
        var whereBody = Expression.Equal(property, value);
        // p => p.{propertyName} == holdpv.v
        var whereLambda = Expression.Lambda<Func<T, bool>>(whereBody, pe);

        // Queryable.Where(query, p => p.{propertyName} == holdpv.v)
        var whereCallExpression = Expression.Call(
            typeof(Queryable),
            "Where",
            new[] { typeof(T) },
            query.Expression,
            whereLambda
        );

        // query.Where(p => p.{propertyName} == holdpv.v)
        return query.Provider.CreateQuery<T>(whereCallExpression);
    }


    /// <summary>
    /// Searches query by predicate and returns entity or throws NotFoundException
    /// </summary>
    /// <param name="source">this query</param>
    /// <param name="predicate">search predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TSource">Type of query's entity</typeparam>
    /// <returns>Entity</returns>
    public static async Task<TSource> GetAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        where TSource : class
    {
        var res = await source.FirstOrDefaultAsync(predicate, cancellationToken);

        if (res == null)
            throw new NotFoundException(DisplayHelper.GetDisplayName<TSource>());

        return res;
    }
}
