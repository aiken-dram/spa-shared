using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Shared.Domain.Attributes;
using Shared.Domain.Models;

namespace Shared.Application.Interfaces;

/// <summary>
/// Helper service for creating audit events
/// </summary>
public interface IAuditService
{
    #region AUDIT EVENT
    /// <summary>
    /// Empty create entity audit event
    /// </summary>
    /// <param name="entity">Auditable entity</param>
    /// <param name="message">Message</param>
    /// <returns>AuditEvent</returns>
    AuditEvent Create(AuditableEntity entity, string? message = null);

    /// <summary>
    /// Create entity audit event
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="message">Message</param>
    /// <returns>AuditEvent</returns>
    Task<AuditEvent> Create<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
        where TEntity : AuditableEntity;

    /// <summary>
    /// Empty edit entity audit event 
    /// </summary>
    /// <param name="entity">Auditable entity</param>
    /// <param name="message">Message</param>
    /// <returns>AuditEvent</returns>
    AuditEvent Edit(AuditableEntity entity, string? message = null);

    /// <summary>
    /// Edit entity audit event
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="message">Message</param>
    /// <returns>AuditEvent</returns>
    Task<AuditEvent> Edit<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
        where TEntity : AuditableEntity;

    /// <summary>
    /// Delete entity audit event
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="message">Message</param>
    /// <returns>AuditEvent</returns>
    AuditEvent Delete(AuditableEntity entity, string? message = null);
    #endregion

    #region AUDIT EVENT DATA LIST
    /// <summary>
    /// Generate list of AuditEventData with values of entity properties that
    /// have [Audit] attribute
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity object</param>
    /// <returns>List of AuditEventData</returns>
    Task<List<AuditEventData>> EntityData<TEntity>(TEntity entity);

    /// <summary>
    /// Generate list of AuditEventData with json from object from expression
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TObject">Type of object</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="objectExpression">Expression from entity returning object</param>
    /// <param name="idType">Dictionary value of EntityDataType</param>
    /// <returns>List of AuditEventData</returns>
    List<AuditEventData> EntityData<TEntity, TObject>(TEntity entity, Expression<Func<TEntity, TObject?>> objectExpression, int idType);

    /// <summary>
    /// Generate list of AuditEventData for editing entity properties with [Audit] attributes
    /// with values in request having same property names
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="request">Request object</param>
    /// <returns>List of AuditEventData</returns>
    Task<List<AuditEventData>> EntityEdit<TEntity, TRequest>(TEntity entity, TRequest request);

    /// <summary>
    /// Generate list of AuditEventData for creating new entity for properties with [Audit] attributes
    /// with values in request having same property names
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="request">Request object</param>
    /// <returns>List of AuditEventData</returns>
    Task<List<AuditEventData>> EntityCreate<TEntity, TRequest>(TEntity entity, TRequest request);
    #endregion

    #region AUDIT EVENT DATA
    /// <summary>
    /// AuditEventData with provided type and object for json serialization
    /// </summary>
    /// <typeparam name="T">Type of object to serialize</typeparam>
    /// <param name="obj">Object to serialize</param>
    /// <param name="type">Id of type in dictionary</param>
    /// <returns>AuditEventData</returns>
    static AuditEventData EventData<T>(T obj, int type) => new AuditEventData(type, JsonConvert.SerializeObject(obj));

    /// <summary>
    /// AuditEventData with Value type and { Value: "val" } json
    /// </summary>
    /// <param name="val">Value</param>
    /// <returns>AuditEventData</returns>
    AuditEventData EventDataValue(string val);

    /// <summary>
    /// AuditEventData with FieldValue type and { Field: "field", Value: "val" } json
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="val">Value</param>
    /// <returns>AuditEventData</returns>
    AuditEventData EventDataFieldValue(string field, string val);

    /// <summary>
    /// AuditEventData with FieldOldNew type and { Field: "field", Old: "old", New: "new" } json
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="oldVal">Old value</param>
    /// <param name="newVal">New value</param>
    /// <returns>AuditEventData</returns>
    AuditEventData EventDataFieldOldNew(string field, string oldVal, string newVal);

    /// <summary>
    /// Audit event data for creating entity with values from request
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="property">Property</param>
    /// <returns>EventDataFieldValue or null</returns>
    Task<AuditEventData?> PropertyCreate<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property);

    /// <summary>
    /// Audit event data for creating entity with values from request of property from expression
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="propertyExpression">Property expression</param>
    /// <returns>AuditEventData or null</returns>
    Task<AuditEventData?> PropertyCreate<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression);

    /// <summary>
    /// AuditEventData if property has changed from entity to request, otherwise null
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="property">PropertyInfo in entity</param>
    /// <returns>AuditEventData or null</returns>
    Task<AuditEventData?> PropertyEdit<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property);

    /// <summary>
    /// AuditEventData if property from expression has changed from entity to request, otherwise null
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <typeparam name="TProperty">Type of property</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="propertyExpression">Property expression</param>
    /// <returns>AuditEventData or null</returns>
    Task<AuditEventData?> PropertyEdit<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression);

    /// <summary>
    /// AuditEventData with value of property
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="property">PropertyInfo in entity</param>
    /// <returns>AuditEventData</returns>
    Task<AuditEventData> PropertyValue<TEntity>(TEntity entity, PropertyInfo property);

    /// <summary>
    /// AuditEventData with value of property from expression
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TProperty">Type of property</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="propertyExpression">Property expression</param>
    /// <returns>AuditEventData</returns>
    Task<AuditEventData> PropertyValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression);
    #endregion

    #region PROPERTY VALUE
    /// <summary>
    /// Return dictionary for provided name
    /// </summary>
    /// <param name="dictionary">Name of dictionary</param>
    /// <returns>Dictionary content</returns>
    Task<Dictionary<object, string>> GetDictionary(string dictionary);

    /// <summary>
    /// Returns string value of property object based on [Audit] attribute
    /// </summary>
    /// <param name="val">Value object</param>
    /// <param name="attr">AuditAttribute</param>
    /// <returns>String value to store in audit event</returns>
    Task<string> PropertyToString(object? val, AuditAttribute attr);

    /// <summary>
    /// Property value from dictionary
    /// </summary>
    /// <param name="val">Id in dictionary</param>
    /// <param name="dictionary">Dictionary name</param>
    /// <returns>Text from dictionary</returns>
    Task<string> PropertyDictionaryValue(object val, string dictionary);
    #endregion
}
