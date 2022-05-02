using Shared.Domain.Interfaces;

namespace Shared.Domain.Models;

/// <summary>
/// Audit event
/// </summary>
public class AuditEvent : IAuditEvent
{
    /// <summary>
    /// Has been logged
    /// </summary>
    public bool IsLogged { get; set; }

    #region INTERFACE
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Stamp { get; set; }

    /// <summary>
    /// Current user
    /// </summary>
    public long IdUser { get; set; }

    /// <summary>
    /// Id of target in dictionary
    /// </summary>
    public int IdTarget { get; set; }

    /// <summary>
    /// Id of action in dictionary
    /// </summary>
    public int IdAction { get; set; }

    /// <summary>
    /// Id of auditable entity
    /// </summary>
    public long? TargetId { get; set; }

    /// <summary>
    /// Name of auditable entity
    /// </summary>
    public string? TargetName { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Event data
    /// </summary>
    public List<IAuditEventData> EventData { get; set; }
    #endregion

    /// <summary>
    /// Constructor from parameters
    /// </summary>
    /// <param name="idTarget">Id of target in dictionary</param>
    /// <param name="idAction">Id of action in dictionary</param>
    /// <param name="targetId">Target id</param>
    /// <param name="targetName">Target name</param>
    /// <param name="message">Message</param>
    public AuditEvent(int idTarget, int idAction, long? targetId, string? targetName, string? message)
    {
        IsLogged = false;
        Stamp = DateTime.Now;
        IdTarget = idTarget;
        IdAction = idAction;
        TargetId = targetId;
        TargetName = targetName;
        Message = message;
        EventData = new List<IAuditEventData>();
    }

    /// <summary>
    /// Constructor from AuditableEntity, 
    /// </summary>
    /// <param name="entity">AuditableEntity</param>
    /// <param name="idAction">Id of action in dictionary</param>
    /// <param name="message">Message</param>
    public AuditEvent(AuditableEntity entity, int idAction, string? message)
    : this(entity.AuditIdTarget, idAction, entity.AuditTargetId, entity.AuditTargetName, message)
    {

    }



    /// <summary>
    /// Add range of AuditEventData
    /// </summary>
    /// <param name="list">List of AuditEventData</param>
    public void AddRange(IList<AuditEventData> list)
    {
        EventData.AddRange(list);
    }

    /// <summary>
    /// Adds a single AuditEventData, if AuditEventData is not null
    /// </summary>
    /// <param name="data">Nullable AuditEventData</param>
    public void Add(AuditEventData? data)
    {
        if (data != null)
            EventData.Add(data);
    }
}

/// <summary>
/// Audit event data
/// </summary>
public class AuditEventData : IAuditEventData
{
    /// <summary>
    /// Id of event data type in dictionary
    /// </summary>
    public int IdType { get; set; }

    /// <summary>
    /// JSON with data parameters
    /// </summary>
    public string? Json { get; set; }

    /// <summary>
    /// Constructor from type and json
    /// </summary>
    /// <param name="idType">Id of type in dictionary</param>
    /// <param name="json">JSON with data parameters</param>
    public AuditEventData(int idType, string json)
    {
        IdType = idType;
        Json = json;
    }
}
