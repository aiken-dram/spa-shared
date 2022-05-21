using System.Linq.Expressions;
using System.Reflection;
using Shared.Application.Exceptions;

namespace Shared.Application.Helpers;

/// <summary>
/// Static class for working with history of editing entity values
/// OBSOLETE once i switch to AuditEvent system
/// </summary>
public static class HistoryHelper
{
    /// <summary>
    /// Format field to save to history
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    private static string? FormatField(object o)
    {
        if (o.GetType() == typeof(DateTime))
            return ((DateTime)o).ToString(Messages.DateTimeFormat);
        if (o.GetType() == typeof(DateTime?))
        {
            var res = (DateTime?)o;
            if (res.HasValue)
                return FormatField(res.Value);
            else
                return null;
        }
        return o.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="fieldName"></param>
    /// <param name="selectExpr"></param>
    /// <param name="value"></param>
    /// <param name="dict"></param>
    /// <param name="NewEntry"></param>
    /// <typeparam name="Tentity"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    /// <returns></returns>
    public static string Edit<Tentity, Tvalue>(ref Tentity entity, string fieldName, Expression<Func<Tentity, Tvalue>> selectExpr, Tvalue value, Tuple<string, string> dict, bool NewEntry = false)
    where Tentity : class
    { return Edit(ref entity, fieldName, selectExpr, value, dict.Item1, dict.Item2, NewEntry); }

    /// <summary>
    /// Changes entity and returns an edit history string
    /// </summary>
    /// <param name="entity">entity</param>
    /// <param name="fieldName">name of edit field</param>
    /// <param name="selectExpr">expression to select property</param>
    /// <param name="value">new value</param>
    /// <param name="formattedOld">formatted old value (for dictionaries)</param>
    /// <param name="formattedNew">formatted new value (for dictionaries)</param>
    /// <param name="NewEntry">Is entity new</param>
    /// <typeparam name="Tentity">type of entity</typeparam>
    /// <typeparam name="Tvalue">type of property</typeparam>
    /// <returns>Edit history string</returns>
    public static string Edit<Tentity, Tvalue>(ref Tentity entity, string fieldName, Expression<Func<Tentity, Tvalue>> selectExpr, Tvalue value, string? formattedOld = null, string? formattedNew = null, bool NewEntry = false)
    where Tentity : class
    {
        var memberExpr = (MemberExpression)selectExpr.Body;
        var property = (PropertyInfo)memberExpr.Member;
        try
        {
            var old = property.GetValue(entity);
            if (old == null && value == null)
                return "";

            if (old == null || !old.Equals(value) || NewEntry)
            {
                property.SetValue(entity, value, null);
                if (formattedOld == null && old != null)
                    formattedOld = FormatField(old);
                if (formattedNew == null && value != null)
                    formattedNew = FormatField(value);

                if (NewEntry)
                    return Messages.HistoryNewValue(fieldName, formattedNew ?? (object?)value ?? "");
                else
                    return Messages.HistoryEditValue(fieldName, formattedOld ?? old ?? Messages.NullValue, formattedNew ?? (object?)value ?? "");
            }
            else
                return "";
        }
        catch (Exception err)
        {
            throw new HistoryEditException(fieldName, err.Message);
        }
    }
}
