using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Shared.Application.Extensions;

/// <summary>
/// Extension for DbSet
/// </summary>
public static class DbSetExtensions
{
    /// <summary>
    /// Returning a string value of function from dictionary entity matching predicate in source DbSet or default
    /// </summary>
    /// <remarks>
    /// If you have dictionary dbset Dictionary { Id, Name }
    /// to get Name string from matching Id value use:
    /// string DictName = await _context.Dictionary.FromDictionaryAsync(p => p.Id == idValue, q => q.Name)
    /// </remarks>
    /// <param name="source">DbSet of dictionary</param>
    /// <param name="predicate">Search predicate</param>
    /// <param name="property">String field function from dictionary</param>
    /// <param name="defaultValue">If source doesnt contain any dictionary entities matching predicate</param>
    /// <param name="defaultKey">Key for dictionary entity if none was found from predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TSource">Dictionary entity type</typeparam>
    /// <returns>Property value from dictionary</returns>
    public static async Task<string?> FromDictionaryAsync<TSource>(this DbSet<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        Func<TSource, string> property,
        string? defaultValue = null,
        object? defaultKey = null,
        CancellationToken cancellationToken = default(CancellationToken))
        where TSource : class
    {
        if (await source.CountAsync(predicate) > 0)
        {
            var d = await source.FirstAsync(predicate, cancellationToken);
            return property(d);
        }
        else
        {
            if (defaultValue != null)
                return defaultValue;
            else if (defaultKey != null)
            {
                var def = await source.FindIdAsync(defaultKey, cancellationToken);
                return (def != null) ? property(def) : null;
            }
            else
                return null;
        }
    }

    /// <summary>
    /// Returning value of function from dictionary entity matching predicate in source DbSet or default
    /// </summary>
    /// <remarks>
    /// If you have dictionary dbset Dictionary { Id, Name }
    /// to get Id from matching Name value use:
    /// string DictName = await _context.Dictionary.DictionaryValueAsync(p => p.Name == nameValue, q => q.Id)
    /// </remarks>
    /// <param name="source">DbSet of dictionary</param>
    /// <param name="predicate">Search predicate</param>
    /// <param name="property">String field function from dictionary</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TSource">Dictionary entity</typeparam>
    /// <returns>Property value from dictionary</returns>
    public static async Task<long> DictionaryValueAsync<TSource>(this DbSet<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        Func<TSource, long> property,
        CancellationToken cancellationToken = default(CancellationToken))
        where TSource : class
    {
        var d = await source.FirstAsync(predicate, cancellationToken);
        return property(d);
    }

    /// <summary>
    /// Returns old and new text values from dictionary based onold and new id values
    /// </summary>
    /// <remarks>
    /// If you have dictionary dbset Dictionary { Id, Name }
    /// and changing value in entity from idOld to idNew
    /// and need to save value changes in history based on dictionary name, not id's
    /// use:
    /// var tuple = await _context.Dictionary.DictionaryHistoryAsync(idOld, idNew, p => p.Name, cancellationToken);
    /// </remarks>
    /// <param name="source">DbSet of dictionary</param>
    /// <param name="oldVal">Old id of dictionary</param>
    /// <param name="newVal">New id of dictionary</param>
    /// <param name="property">string field function from dictionary</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TSource">Dictionary entity</typeparam>
    /// <returns>Old and new property values from dictionary</returns>
    public static async Task<Tuple<string, string>> DictionaryHistoryAsync<TSource>(this DbSet<TSource> source,
        long? oldVal,
        long? newVal,
        Func<TSource, string> property,
        CancellationToken cancellationToken = default(CancellationToken))
        where TSource : class
    {
        string oldDict = Messages.NullValue;
        if (oldVal.HasValue)
        {
            var dOld = await source.FindIdAsync(oldVal.Value, cancellationToken);
            if (dOld != null)
                oldDict = property(dOld);
        }

        string newDict = Messages.NullValue;
        if (newVal.HasValue)
        {
            var dNew = await source.FindIdAsync(newVal.Value, cancellationToken);
            if (dNew != null)
                newDict = property(dNew);
        }

        return new Tuple<string, string>(oldDict, newDict);
    }

    /// <summary>
    /// Rewrites FindAsync for simplier syntax
    /// </summary>
    /// <param name="source">this DbSet</param>
    /// <param name="key">object key</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <typeparam name="TSource">type of dbset</typeparam>
    /// <returns>FindAsync</returns>
    public static ValueTask<TSource?> FindIdAsync<TSource>(this DbSet<TSource> source, object? key, CancellationToken cancellationToken)
        where TSource : class
        => source.FindAsync(new object?[] { key }, cancellationToken);
}
