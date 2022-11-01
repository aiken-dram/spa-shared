using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Application.Extensions;
using Shared.Application.Interfaces;
using Shared.Domain.Attributes;
using Shared.Domain.Enums;
using Shared.Domain.Models;

namespace Shared.Application.Services;

#region OPTIONS
public class AuditDataTypeOptions
{
    /// <summary>
    /// Dictionary for { Value: "value" } json
    /// </summary>
    public int dataTypeValue { get; set; } = 1;

    /// <summary>
    /// Dictionary for { Field: "field", Value: "value" } json
    /// </summary>
    public int dataTypeFieldValue { get; set; } = 2;

    /// <summary>
    /// Dictionary for { Field: "field", Old: "old", "New": "new" } json
    /// </summary>
    public int dataTypeFieldOldNew { get; set; } = 3;

    /// <summary>
    /// Dictionary for { Field: "field", Operation: "operation", "Value": "value" } json
    /// </summary>
    public int dataTypeFieldOperationValue { get; set; } = 4;
}

public class AuditActionOptions
{
    /// <summary>
    /// Create action
    /// </summary>
    public int actionCreate { get; set; } = 1;

    /// <summary>
    /// Edit action
    /// </summary>
    public int actionEdit { get; set; } = 2;

    /// <summary>
    /// Delete action
    /// </summary>
    public int actionDelete { get; set; } = 3;
}

public class AuditOptionsBuilder
{
    /// <summary>
    /// Options for audit entity data type
    /// </summary>
    public AuditDataTypeOptions DataTypeOptions { get; set; } = new AuditDataTypeOptions();

    /// <summary>
    /// Options for audit actions
    /// </summary>
    public AuditActionOptions ActionOptions { get; set; } = new AuditActionOptions();
}
#endregion

/// <summary>
/// Abstract builder for creating AuditData
/// Override method for retrieving dictionaries
/// </summary>
public abstract class AuditBuilder : IAuditBuilder
{
    public AuditBuilder(Action<AuditOptionsBuilder>? options = null)
    {
        AuditOptionsBuilder o = new AuditOptionsBuilder();
        if (options != null)
            options(o);

        dataTypeValue = o.DataTypeOptions.dataTypeValue;
        dataTypeFieldValue = o.DataTypeOptions.dataTypeFieldValue;
        dataTypeFieldOldNew = o.DataTypeOptions.dataTypeFieldOldNew;
        dataTypeFieldOperationValue = o.DataTypeOptions.dataTypeFieldOperationValue;

        actionCreate = o.ActionOptions.actionCreate;
        actionEdit = o.ActionOptions.actionEdit;
        actionDelete = o.ActionOptions.actionDelete;
    }

    #region OPTIONS
    private readonly int dataTypeValue;
    private readonly int dataTypeFieldValue;
    private readonly int dataTypeFieldOldNew;
    private readonly int dataTypeFieldOperationValue;

    private readonly int actionCreate;
    private readonly int actionEdit;
    private readonly int actionDelete;
    #endregion

    #region PRIVATE STATIC
    /// <summary>
    /// List of properties in entity that have [Audit] attribute
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    private static IEnumerable<PropertyInfo> AuditProperties<TEntity>()
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
    private static AuditAttribute GetAuditAttribute(PropertyInfo p)
    {
        return (AuditAttribute?)Attribute.GetCustomAttribute(p, typeof(AuditAttribute)) ?? new AuditAttribute();
    }

    /// <summary>
    /// PropertyInfo in request with same name as PropertyInfo in entity, null if there isnt one
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="p">PropertyInfo in entity or null</param>
    private static PropertyInfo? RequestProperty<TRequest>(PropertyInfo p)
    {
        var t = typeof(TRequest);
        return t.GetProperties().FirstOrDefault(r => r.Name == p.Name);
    }
    #endregion

    /// <summary>
    /// Dictionaries in audit
    /// </summary>
    public Dictionary<string, Dictionary<long, string?>> Dictionaries = new Dictionary<string, Dictionary<long, string?>>();

    #region AUDIT DATA CONSTRUCTORS 
    public AuditData DataValue(string val)
    {
        var obj = new JsonDataValue
        {
            Value = val
        };
        return IAuditBuilder.Data(obj, dataTypeValue);
    }

    public AuditData DataFieldValue(string field, string val)
    {
        var obj = new JsonDataFieldValue
        {
            Field = field,
            Value = val
        };
        return IAuditBuilder.Data(obj, dataTypeFieldValue);
    }

    public AuditData DataFieldOldNew(string field, string oldVal, string newVal)
    {
        var obj = new JsonDataFieldOldNew
        {
            Field = field,
            Old = oldVal,
            New = newVal
        };
        return IAuditBuilder.Data(obj, dataTypeFieldOldNew);
    }

    public AuditData DataFieldOperationValue(string field, string operation, string value)
    {
        var obj = new JsonDataFieldOperationValue
        {
            Field = field,
            Operation = operation,
            Value = value
        };
        return IAuditBuilder.Data(obj, dataTypeFieldOperationValue);
    }
    #endregion

    #region AUDIT DATA FOR PROPERTY AUDIT
    public async Task<AuditData?> PropertyCreateAsync<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property)
    {
        //request property
        var r = RequestProperty<TRequest>(property);
        var attr = GetAuditAttribute(property);
        //if there's property in request with same name
        if (r != null)
        {
            //request value
            var rVal = await ToStringAsync(request, r, attr);

            return DataFieldValue(property.Name, rVal);
        }
        return null;
    }

    public Task<AuditData?> PropertyCreateAsync<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression)
    => PropertyCreateAsync(entity, request, propertyExpression.GetPropertyInfo());

    public async Task<AuditData?> PropertyCreateAsync<TEntity>(EntityEntry<TEntity> entry, PropertyInfo property)
        where TEntity : class
    {
        //request property
        var attr = GetAuditAttribute(property);

        //current value
        var currentValue = entry.Property(property.Name).CurrentValue;
        var val = await PropertyToStringAsync(currentValue, attr);

        return DataFieldValue(property.Name, val);
    }

    public async Task<AuditData?> PropertyEditAsync<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property)
    {
        var attr = GetAuditAttribute(property);

        //old (entity) value
        var oVal = await ToStringAsync(entity, property, attr);

        //request property
        var r = RequestProperty<TRequest>(property);
        //if there's property in request with same name
        if (r != null)
        {
            //new (request) value
            var nVal = await ToStringAsync(request, r, attr);

            //if not equal return AuditData
            if (nVal != oVal)
                return DataFieldOldNew(property.Name, oVal, nVal);
        }
        return null;
    }

    public Task<AuditData?> PropertyEditAsync<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression)
    => PropertyEditAsync(entity, request, propertyExpression.GetPropertyInfo());

    private async Task<AuditData?> PropertyEditAsync<TEntity>(EntityEntry<TEntity> entry, PropertyInfo property)
        where TEntity : class
    {
        var attr = GetAuditAttribute(property);

        var originalValue = entry.Property(property.Name).OriginalValue;
        var currentValue = entry.Property(property.Name).CurrentValue;

        var oVal = await PropertyToStringAsync(originalValue, attr);
        var nVal = await PropertyToStringAsync(currentValue, attr);

        if (oVal != nVal)
            return DataFieldOldNew(property.Name, oVal, nVal);

        return null;
    }

    public async Task<AuditData> PropertyValueAsync<TEntity>(TEntity entity, PropertyInfo property)
    {
        var attr = GetAuditAttribute(property);
        var val = await ToStringAsync(entity, property, attr);
        return DataFieldValue(property.Name, val);
    }

    public Task<AuditData> PropertyValueAsync<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression)
    => PropertyValueAsync(entity, propertyExpression.GetPropertyInfo());
    #endregion

    #region PROPERTY VALUE
    public abstract Task<Dictionary<long, string?>> GetDictionaryAsync(string dictionary);

    public virtual async Task<string> PropertyDictionaryValueAsync(object val, string dictionary)
    {
        //populate dictionary if missing one with this name
        if (!Dictionaries.ContainsKey(dictionary))
            Dictionaries.Add(dictionary, await GetDictionaryAsync(dictionary));

        //might not be long
        long v = Convert.ToInt64(val);

        var dict = Dictionaries[dictionary];
        if (dict.ContainsKey(v))
            return dict[v] ?? Messages.NullValue;
        else
            return val.ToString() ?? Messages.NullValue;
    }

    public virtual async Task<string> PropertyToStringAsync(object? val, AuditAttribute attr)
    {
        switch (val)
        {
            case short when attr.Dictionary != null:
            case int when attr.Dictionary != null:
            case long when attr.Dictionary != null:
            case Enum when attr.Dictionary != null:
                return await PropertyDictionaryValueAsync(val, attr.Dictionary);

            case string s when attr.IsCharBoolean:
                return s == CharBoolean.True ? Messages.CharBooleanTrue : Messages.CharBooleanFalse;

            case string when attr.HideValue:
                return Messages.HiddenValue;

            case decimal c when attr.IsCurreny:
                return c.Currency();

            case DateTime t when attr.IsTimeStamp:
                return t.ToString(Messages.TimestampFormat);

            case DateTime d:
                return d.ToString(Messages.DateTimeFormat);
        }
        return val?.ToString() ?? Shared.Application.Messages.NullValue;
    }

    /// <summary>
    /// Get string value of object's property with provided AuditAttribute parameters
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="obj">Object</param>
    /// <param name="p">PropertyInfo</param>
    /// <param name="attr">AuditAttribute</param>
    /// <returns>String value</returns>
    private async Task<string> ToStringAsync<T>(T obj, PropertyInfo p, AuditAttribute attr)
    {
        var val = p.GetValue(obj);
        return await PropertyToStringAsync(val, attr);
    }
    #endregion

    #region AUDIT DATA LIST CONSTRUCTORS
    public async Task<List<AuditData>> EntityDataAsync<TEntity>(TEntity entity)
    {
        var res = new List<AuditData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditProperties<TEntity>())
            res.Add(await PropertyValueAsync(entity, p));

        return res;
    }

    public List<AuditData> EntityData<TEntity, TObject>(TEntity entity, Expression<Func<TEntity, TObject?>> objectExpression, int idType)
    {
        var res = new List<AuditData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditProperties<TEntity>())
        {
            var obj = objectExpression.Compile()(entity);
            if (obj != null)
                res.Add(IAuditBuilder.Data(obj, idType));
        }
        return res;
    }

    public async Task<List<AuditData>> EntityEditAsync<TEntity, TRequest>(TEntity entity, TRequest request)
    {
        var res = new List<AuditData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditProperties<TEntity>())
        {
            var d = await PropertyEditAsync(entity, request, p);
            if (d != null)
                res.Add(d);
        }
        return res;
    }

    private async Task<List<AuditData>> EntryEditAsync<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : class
    {
        var res = new List<AuditData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditProperties<TEntity>())
        {
            var d = await PropertyEditAsync(entry, p);
            if (d != null)
                res.Add(d);
        }
        return res;
    }

    public async Task<List<AuditData>> EntityCreateAsync<TEntity, TRequest>(TEntity entity, TRequest request)
    {
        var res = new List<AuditData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditProperties<TEntity>())
        {
            var d = await PropertyCreateAsync(entity, request, p);
            if (d != null)
                res.Add(d);
        }
        return res;
    }

    private async Task<List<AuditData>> EntryCreateAsync<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : class
    {
        var res = new List<AuditData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditProperties<TEntity>())
        {
            var d = await PropertyCreateAsync(entry, p);
            if (d != null)
                res.Add(d);
        }
        return res;
    }
    #endregion

    #region AUDIT CONSTUCTORS
    /// <remarks>
    /// gonna skip TargetId for these general ones when creating,
    /// since i wont have entity.Id when creating new entity til after savechanges
    /// later in specific audit entities i'll have foreign key to parent object
    /// and will add these not to UserAudit, but to %EntityName%Audit
    /// </remarks>
    public Audit Create(AuditableEntity entity, string? message = null)
    => new Audit(entity.AuditIdTarget, actionCreate, null, entity.AuditTargetName, message);

    public async Task<Audit> CreateAsync<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
    where TEntity : AuditableEntity
    {
        var res = Create((AuditableEntity)entity, message);
        // AuditData
        res.AddRange(await EntityCreateAsync(entity, request));
        return res;
    }

    public async Task<Audit> CreateAsync<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : AuditableEntity
    {
        var res = Create((AuditableEntity)entry.Entity, "AutoAudit");

        // AuditData
        res.AddRange(await EntryCreateAsync(entry));

        return res;
    }

    public Audit Edit(AuditableEntity entity, string? message = null)
    => new Audit(entity.AuditIdTarget, actionEdit, entity.AuditTargetId, entity.AuditTargetName, message);

    public async Task<Audit> EditAsync<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
    where TEntity : AuditableEntity
    {
        var res = Edit((AuditableEntity)entity, message);
        // AuditData
        res.AddRange(await EntityEditAsync(entity, request));
        return res;
    }

    public async Task<Audit> EditAsync<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : AuditableEntity
    {
        var res = Edit((AuditableEntity)entry.Entity, "AutoAudit");
        // AuditData
        res.AddRange(await EntryEditAsync(entry));
        return res;
    }

    public Audit Delete(AuditableEntity entity, string? message = null)
    => new Audit(entity.AuditIdTarget, actionDelete, null, entity.AuditTargetName, message);

    public Audit Delete<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : AuditableEntity
    {
        var entity = entry.Entity;
        var res = Delete((AuditableEntity)entity, "AutoAudit");
        return res;
    }
    #endregion
}