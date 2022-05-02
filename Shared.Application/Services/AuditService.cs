using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Application.Extensions;
using Shared.Application.Helpers;
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

public class AuditOptions
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
/// Abstract service for creating AuditEventData
/// </summary>
public abstract class AuditService : IAuditService
{
    public AuditService(IOptions<AuditOptions> options)
    {
        dataTypeValue = options.Value.DataTypeOptions.dataTypeValue;
        dataTypeFieldValue = options.Value.DataTypeOptions.dataTypeFieldValue;
        dataTypeFieldOldNew = options.Value.DataTypeOptions.dataTypeFieldOldNew;

        actionCreate = options.Value.ActionOptions.actionCreate;
        actionEdit = options.Value.ActionOptions.actionEdit;
        actionDelete = options.Value.ActionOptions.actionDelete;
    }

    #region OPTIONS
    private readonly int dataTypeValue;
    private readonly int dataTypeFieldValue;
    private readonly int dataTypeFieldOldNew;

    private readonly int actionCreate;
    private readonly int actionEdit;
    private readonly int actionDelete;
    #endregion

    /// <summary>
    /// Dictionaries in audit
    /// </summary>
    public Dictionary<string, Dictionary<object, string>> Dictionaries = new Dictionary<string, Dictionary<object, string>>();

    #region AUDIT EVENT DATA CONSTRUCTORS 
    public AuditEventData EventDataValue(string val)
    {
        var obj = new JsonEventDataValue
        {
            Value = val
        };
        return IAuditService.EventData(obj, dataTypeValue);
    }

    public AuditEventData EventDataFieldValue(string field, string val)
    {
        var obj = new JsonEventDataFieldValue
        {
            Field = field,
            Value = val
        };
        return IAuditService.EventData(obj, dataTypeFieldValue);
    }

    public AuditEventData EventDataFieldOldNew(string field, string oldVal, string newVal)
    {
        var obj = new JsonEventDataFieldOldNew
        {
            Field = field,
            Old = oldVal,
            New = newVal
        };
        return IAuditService.EventData(obj, dataTypeFieldOldNew);
    }
    #endregion

    #region AUDIT EVENT DATA FOR PROPERTY AUDIT
    public async Task<AuditEventData?> PropertyCreate<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property)
    {
        //request property
        var r = AuditHelper.RequestProperty<TRequest>(property);
        var attr = AuditHelper.GetAuditAttribute(property);
        //if there's property in request with same name
        if (r != null)
        {
            //request value
            var rVal = await ToString(request, r, attr);

            return EventDataFieldValue(property.Name, rVal);
        }
        return null;
    }

    public Task<AuditEventData?> PropertyCreate<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression)
    => PropertyCreate(entity, request, propertyExpression.GetPropertyInfo());

    public async Task<AuditEventData?> PropertyEdit<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property)
    {
        var attr = AuditHelper.GetAuditAttribute(property);

        //entity value
        var eVal = await ToString(entity, property, attr);

        //request property
        var r = AuditHelper.RequestProperty<TRequest>(property);
        //if there's property in request with same name
        if (r != null)
        {
            //request value
            var rVal = await ToString(request, r, attr);

            //if not equal return AuditEventData
            if (eVal != rVal)
                return EventDataFieldOldNew(property.Name, eVal, rVal);
        }
        return null;
    }

    public Task<AuditEventData?> PropertyEdit<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression)
    => PropertyEdit(entity, request, propertyExpression.GetPropertyInfo());

    public async Task<AuditEventData> PropertyValue<TEntity>(TEntity entity, PropertyInfo property)
    {
        var attr = AuditHelper.GetAuditAttribute(property);
        var val = await ToString(entity, property, attr);
        return EventDataFieldValue(property.Name, val);
    }

    public Task<AuditEventData> PropertyValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression)
    => PropertyValue(entity, propertyExpression.GetPropertyInfo());
    #endregion

    #region PROPERTY VALUE
    public abstract Task<Dictionary<object, string>> GetDictionary(string dictionary);

    public virtual async Task<string> PropertyDictionaryValue(object val, string dictionary)
    {
        //populate dictionary if missing one with this name
        if (!Dictionaries.ContainsKey(dictionary))
            Dictionaries.Add(dictionary, await GetDictionary(dictionary));

        var dict = Dictionaries[dictionary];
        if (dict.ContainsKey(val))
            return dict[val];
        else
            return val.ToString() ?? Messages.NullValue;
    }

    public virtual async Task<string> PropertyToString(object? val, AuditAttribute attr)
    {
        switch (val)
        {
            case short when attr.Dictionary != null:
            case int when attr.Dictionary != null:
            case long when attr.Dictionary != null:
                return await PropertyDictionaryValue(val, attr.Dictionary);

            case string s when attr.IsCharBoolean:
                return s == CharBoolean.True ? Messages.CharBooleanTrue : Messages.CharBooleanFalse;

            case decimal c when attr.IsCurreny:
                return DisplayHelper.Currency(c);

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
    private async Task<string> ToString<T>(T obj, PropertyInfo p, AuditAttribute attr)
    {
        var val = p.GetValue(obj);
        return await PropertyToString(val, attr);
    }
    #endregion

    #region AUDIT EVENT DATA LIST CONSTRUCTORS
    public async Task<List<AuditEventData>> EntityData<TEntity>(TEntity entity)
    {
        var res = new List<AuditEventData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditHelper.AuditProperties<TEntity>())
            res.Add(await PropertyValue(entity, p));

        return res;
    }

    public List<AuditEventData> EntityData<TEntity, TObject>(TEntity entity, Expression<Func<TEntity, TObject?>> objectExpression, int idType)
    {
        var res = new List<AuditEventData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditHelper.AuditProperties<TEntity>())
        {
            var obj = objectExpression.Compile()(entity);
            if (obj != null)
                res.Add(IAuditService.EventData(obj, idType));
        }
        return res;
    }

    public async Task<List<AuditEventData>> EntityEdit<TEntity, TRequest>(TEntity entity, TRequest request)
    {
        var res = new List<AuditEventData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditHelper.AuditProperties<TEntity>())
        {
            var d = await PropertyEdit(entity, request, p);
            if (d != null)
                res.Add(d);
        }
        return res;
    }

    public async Task<List<AuditEventData>> EntityCreate<TEntity, TRequest>(TEntity entity, TRequest request)
    {
        var res = new List<AuditEventData>();

        //loop for all properties with [Audit] attribute
        foreach (var p in AuditHelper.AuditProperties<TEntity>())
        {
            var d = await PropertyCreate(entity, request, p);
            if (d != null)
                res.Add(d);
        }
        return res;
    }
    #endregion

    #region AUDIT EVENT CONSTUCTORS
    /// <remarks>
    /// lets skip TargetId for these general ones when creating,
    /// since i wont have entity.Id when creating new entity until after savechanges
    /// later in specific audit entities i'll have foreign key
    /// and will add these event not to user, but to specific entity
    /// </remarks>
    public AuditEvent Create(AuditableEntity entity, string? message = null)
    => new AuditEvent(entity.AuditIdTarget, actionCreate, null, entity.AuditTargetName, message);

    public async Task<AuditEvent> Create<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
    where TEntity : AuditableEntity
    {
        var res = Create((AuditableEntity)entity, message);
        // EventData
        res.AddRange(await EntityCreate(entity, request));
        return res;
    }

    public AuditEvent Edit(AuditableEntity entity, string? message = null)
    => new AuditEvent(entity.AuditIdTarget, actionEdit, entity.AuditTargetId, entity.AuditTargetName, message);

    public async Task<AuditEvent> Edit<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
    where TEntity : AuditableEntity
    {
        var res = Edit((AuditableEntity)entity, message);
        // Event data
        res.AddRange(await EntityEdit(entity, request));
        return res;
    }

    public AuditEvent Delete(AuditableEntity entity, string? message = null)
    => new AuditEvent(entity.AuditIdTarget, actionDelete, null, entity.AuditTargetName, message);
    #endregion
}