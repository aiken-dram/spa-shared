using Shared.Domain.Interfaces;

namespace Shared.Domain.Models;

/// <summary>
/// Audit log
/// </summary>
public class Audit : IAudit
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
    /// Audit data
    /// </summary>
    public List<IAuditData> AuditData { get; set; }
    #endregion

    /// <summary>
    /// Constructor from parameters
    /// </summary>
    /// <param name="idTarget">Id of target in dictionary</param>
    /// <param name="idAction">Id of action in dictionary</param>
    /// <param name="targetId">Target id</param>
    /// <param name="targetName">Target name</param>
    /// <param name="message">Message</param>
    public Audit(int idTarget, int idAction, long? targetId, string? targetName, string? message)
    {
        IsLogged = false;
        Stamp = DateTime.Now;
        IdTarget = idTarget;
        IdAction = idAction;
        TargetId = targetId;
        TargetName = targetName;
        Message = message;
        AuditData = new List<IAuditData>();
    }

    /// <summary>
    /// Constructor from AuditableEntity
    /// </summary>
    /// <param name="entity">AuditableEntity</param>
    /// <param name="idAction">Id of action in dictionary</param>
    /// <param name="message">Message</param>
    public Audit(AuditableEntity entity, int idAction, string? message)
    : this(entity.AuditIdTarget, idAction, entity.AuditTargetId, entity.AuditTargetName, message)
    {

    }

    /// <summary>
    /// Add range of AuditData
    /// </summary>
    /// <param name="list">List of AuditData</param>
    public void AddRange(IList<AuditData> list)
    {
        AuditData.AddRange(list);
    }

    /// <summary>
    /// Adds a single AuditData, if AuditData is not null
    /// </summary>
    /// <param name="data">Nullable AuditData</param>
    public void Add(AuditData? data)
    {
        if (data != null)
            AuditData.Add(data);
    }
}

/// <summary>
/// Audit log data
/// </summary>
public class AuditData : IAuditData
{
    /// <summary>
    /// Id of audit data type in dictionary
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
    public AuditData(int idType, string json)
    {
        IdType = idType;
        Json = json;
    }
}
