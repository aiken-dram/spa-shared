using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Shared.Domain.Attributes;
using Shared.Domain.Models;

namespace Shared.Application.Interfaces;

/// <summary>
/// Helper service for creating audit logs
/// </summary>
public interface IAuditBuilder
{
    #region AUDIT
    /// <summary>
    /// Empty create entity audit for custom audit
    /// </summary>
    /// <param name="entity">Auditable entity</param>
    /// <param name="message">Message</param>
    /// <returns>Audit</returns>
    Audit Create(AuditableEntity entity, string? message = null);

    /// <summary>
    /// Create entity audit
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="message">Message</param>
    /// <returns>Audit</returns>
    Task<Audit> Create<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
        where TEntity : AuditableEntity;

    /// <summary>
    /// Create entity audit for automatic audit with entity framework
    /// </summary>
    /// <param name="entry">Entity</param>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <returns>Audit</returns>
    Task<Audit> Create<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : AuditableEntity;

    /// <summary>
    /// Empty edit entity audit for custom audit
    /// </summary>
    /// <param name="entity">Auditable entity</param>
    /// <param name="message">Message</param>
    /// <returns>Audit</returns>
    Audit Edit(AuditableEntity entity, string? message = null);

    /// <summary>
    /// Edit entity audit
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="message">Message</param>
    /// <returns>Audit</returns>
    Task<Audit> Edit<TEntity, TRequest>(TEntity entity, TRequest request, string? message = null)
        where TEntity : AuditableEntity;

    /// <summary>
    /// Edit entity audit for automatic audit with entity framework
    /// </summary>
    /// <param name="entry">Entity</param>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <returns>Audit</returns>
    Task<Audit> Edit<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : AuditableEntity;

    /// <summary>
    /// Delete entity audit
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="message">Message</param>
    /// <returns>Audit</returns>
    Audit Delete(AuditableEntity entity, string? message = null);

    /// <summary>
    /// Delete entity audit for automatic audit with entity framework
    /// </summary>
    /// <param name="entry">ef core EntityEntry</param>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <returns>Audit</returns>
    Audit Delete<TEntity>(EntityEntry<TEntity> entry)
        where TEntity : AuditableEntity;
    #endregion

    #region AUDIT DATA LIST
    /// <summary>
    /// Generate list of AuditData with values of entity properties that
    /// have [Audit] attribute
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity object</param>
    /// <returns>List of AuditData</returns>
    Task<List<AuditData>> EntityData<TEntity>(TEntity entity);

    /// <summary>
    /// Generate list of AuditData with json from object from expression
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TObject">Type of object</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="objectExpression">Expression from entity returning object</param>
    /// <param name="idType">Dictionary value of EntityDataType</param>
    /// <returns>List of AuditData</returns>
    List<AuditData> EntityData<TEntity, TObject>(TEntity entity, Expression<Func<TEntity, TObject?>> objectExpression, int idType);

    /// <summary>
    /// Generate list of AuditData for editing entity properties with [Audit] attributes
    /// with values in request having same property names
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="request">Request object</param>
    /// <returns>List of AuditData</returns>
    Task<List<AuditData>> EntityEdit<TEntity, TRequest>(TEntity entity, TRequest request);

    /// <summary>
    /// Generate list of AuditData for creating new entity for properties with [Audit] attributes
    /// with values in request having same property names
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="entity">Entity object</param>
    /// <param name="request">Request object</param>
    /// <returns>List of AuditData</returns>
    Task<List<AuditData>> EntityCreate<TEntity, TRequest>(TEntity entity, TRequest request);
    #endregion

    #region AUDIT DATA
    /// <summary>
    /// AuditData with provided type and object for json serialization
    /// </summary>
    /// <typeparam name="T">Type of object to serialize</typeparam>
    /// <param name="obj">Object to serialize</param>
    /// <param name="type">Id of type in dictionary</param>
    /// <returns>AuditData</returns>
    static AuditData Data<T>(T obj, int type) => new AuditData(type, JsonConvert.SerializeObject(obj));

    /// <summary>
    /// AuditData with Value type and { Value: "val" } json
    /// </summary>
    /// <param name="val">Value</param>
    /// <returns>AuditData</returns>
    AuditData DataValue(string val);

    /// <summary>
    /// AuditData with FieldValue type and { Field: "field", Value: "val" } json
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="val">Value</param>
    /// <returns>AuditData</returns>
    AuditData DataFieldValue(string field, string val);

    /// <summary>
    /// AuditData with FieldOldNew type and { Field: "field", Old: "old", New: "new" } json
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="oldVal">Old value</param>
    /// <param name="newVal">New value</param>
    /// <returns>AuditData</returns>
    AuditData DataFieldOldNew(string field, string oldVal, string newVal);

    /// <summary>
    /// Audit event data for creating entity with values from request
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="property">Property</param>
    /// <returns>EventDataFieldValue or null</returns>
    Task<AuditData?> PropertyCreate<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property);

    /// <summary>
    /// Audit event data for creating entity with values from request of property from expression
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="propertyExpression">Property expression</param>
    /// <returns>AuditData or null</returns>
    Task<AuditData?> PropertyCreate<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression);

    /// <summary>
    /// AuditData if property has changed from entity to request, otherwise null
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="property">PropertyInfo in entity</param>
    /// <returns>AuditData or null</returns>
    Task<AuditData?> PropertyEdit<TEntity, TRequest>(TEntity entity, TRequest request, PropertyInfo property);

    /// <summary>
    /// AuditData if property from expression has changed from entity to request, otherwise null
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <typeparam name="TProperty">Type of property</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="request">Request</param>
    /// <param name="propertyExpression">Property expression</param>
    /// <returns>AuditData or null</returns>
    Task<AuditData?> PropertyEdit<TEntity, TRequest, TProperty>(TEntity entity, TRequest request, Expression<Func<TEntity, TProperty>> propertyExpression);

    /// <summary>
    /// AuditData with value of property
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="property">PropertyInfo in entity</param>
    /// <returns>AuditData</returns>
    Task<AuditData> PropertyValue<TEntity>(TEntity entity, PropertyInfo property);

    /// <summary>
    /// AuditData with value of property from expression
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TProperty">Type of property</typeparam>
    /// <param name="entity">Entity</param>
    /// <param name="propertyExpression">Property expression</param>
    /// <returns>AuditData</returns>
    Task<AuditData> PropertyValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression);
    #endregion

    #region PROPERTY VALUE
    /// <summary>
    /// Return dictionary for provided name
    /// </summary>
    /// <param name="dictionary">Name of dictionary</param>
    /// <returns>Dictionary content</returns>
    Task<Dictionary<object, string?>> GetDictionary(string dictionary);

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
