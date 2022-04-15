using System.Reflection;

namespace Shared.Application.Extensions;

/// <summary>
/// Extensions for Object and IDictionary conversions
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts IDictionary to object of T class
    /// </summary>
    /// <param name="source">Provided IDictionary for conversion</param>
    /// <typeparam name="T">Object class</typeparam>
    /// <returns>Object of T class converted from IDictionary</returns>
    public static T? ToObject<T>(this IDictionary<string, object?> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                .GetProperty(item.Key)?
                .SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    /// <summary>
    /// Converts object to IDictionary
    /// </summary>
    /// <param name="source">Object</param>
    /// <param name="bindingAttr">Binding attributes for creating IDictionary</param>
    /// <returns>IDictionary converted from provided object</returns>
    public static IDictionary<string, object?> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );
    }
}
