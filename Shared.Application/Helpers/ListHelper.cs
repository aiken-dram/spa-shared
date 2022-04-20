namespace Shared.Application.Helpers;

/// <summary>
/// Static class for working with List
/// </summary>
public static class ListHelper
{
    /// <summary>
    /// Returns list of id which are to be added to dbcontext
    /// </summary>
    /// <param name="requestId">id of entity</param>
    /// <param name="list">provided list</param>
    /// <param name="existing">existing list</param>
    /// <returns>list to be added to dbcontext</returns>
    public static IEnumerable<long>? AddList(long? requestId, long[]? list, List<long>? existing)
    {
        if (requestId.HasValue)
        {
            if (existing == null || existing.Count == 0)
            {
                //add all to entity that has no list
                return list;
            }
            else
            {
                if (list == null || list.Length == 0)
                {
                    //add none
                    return null;
                }
                else
                {
                    //need to select from list which are not in existing
                    return list.Where(p => existing.Count(q => q == p) == 0);
                }
            }
        }
        else
        {
            //new entity, add all
            return list;
        }
    }

    /// <summary>
    /// Returns list of id which are to be removed from dbcontext
    /// </summary>
    /// <param name="requestId">id of entity</param>
    /// <param name="list">provided list</param>
    /// <param name="existing">existing list</param>
    /// <returns>list to be removed from dbcontext</returns>
    public static IEnumerable<long>? RemoveList(long? requestId, long[]? list, List<long>? existing)
    {
        if (requestId.HasValue)
        {
            if (existing == null || existing.Count == 0)
            {
                //remove none from entity that has no list
                return null;
            }
            else
            {
                if (list == null || list.Length == 0)
                {
                    //remove all
                    return existing;
                }
                else
                {
                    //need to select existing that are not in list
                    return existing.Where(p => list.Count(q => q == p) == 0);
                }
            }
        }
        else
        {
            //new entity, remove none
            return null;
        }
    }

    /// <summary>
    /// Splits original list into two based on predicate
    /// </summary>
    /// <param name="source">Original list</param>
    /// <param name="predicate">Condition to split list</param>
    /// <typeparam name="T">type of list</typeparam>
    /// <returns>Pair of lists (condition is true), (condition is false)</returns>
    public static Tuple<IEnumerable<T>, IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        var left = new List<T>();
        var right = new List<T>();

        foreach (var item in source)
        {
            if (predicate(item))
            {
                left.Add(item);
            }
            else
            {
                right.Add(item);
            }
        }

        return new Tuple<IEnumerable<T>, IEnumerable<T>>(left, right);
    }
}
