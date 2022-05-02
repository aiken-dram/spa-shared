using System.Reflection;
using Shared.Domain.Attributes;

namespace Shared.Application.Helpers;

public static class AuditHelper
{
    /// <summary>
    /// List of properties in entity that have [Audit] attribute
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public static IEnumerable<PropertyInfo> AuditProperties<TEntity>()
    {
        var t = typeof(TEntity);
        var props = t.GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(AuditAttribute)));
        return props;
    }

    /// <summary>
    /// Get audit attribute from entity property info
    /// </summary>
    /// <param name="p">PropertyInfo of entity</param>
    /// <returns>AuditAttribute</returns>
    public static AuditAttribute GetAuditAttribute(PropertyInfo p)
    {
        return (AuditAttribute?)Attribute.GetCustomAttribute(p, typeof(AuditAttribute)) ?? new AuditAttribute();
    }

    /// <summary>
    /// PropertyInfo in request with same name as PropertyInfo in entity, null if there isnt one
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="p">PropertyInfo in entity or null</param>
    public static PropertyInfo? RequestProperty<TRequest>(PropertyInfo p)
    {
        var t = typeof(TRequest);
        return t.GetProperties().FirstOrDefault(r => r.Name == p.Name);
    }
}
