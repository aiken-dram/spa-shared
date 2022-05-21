namespace Shared.Domain.Interfaces;

/// <summary>
/// Interface for audit log
/// </summary>
public interface IAudit
{
    /// <summary>
    /// Date and time of audit
    /// </summary>
    DateTime Stamp { get; set; }

    /// <summary>
    /// Id of user
    /// </summary>
    long IdUser { get; set; }

    /// <summary>
    /// Id of target
    /// </summary>
    int IdTarget { get; set; }

    /// <summary>
    /// Id of action
    /// </summary>
    int IdAction { get; set; }

    /// <summary>
    /// Id of target entity
    /// </summary>
    long? TargetId { get; set; }

    /// <summary>
    /// Name of target entity
    /// </summary>
    string? TargetName { get; set; }

    /// <summary>
    /// Audit message
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    /// Audit data
    /// </summary>
    List<IAuditData> AuditData { get; set; }
}

/// <summary>
/// Interface for audit log data
/// </summary>
public interface IAuditData
{
    /// <summary>
    /// Id of data type
    /// </summary>
    int IdType { get; set; }

    /// <summary>
    /// Json with data
    /// </summary>
    string? Json { get; set; }
}