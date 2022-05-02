namespace Shared.Domain.Interfaces;

/// <summary>
/// Interface for audit event
/// </summary>
public interface IAuditEvent
{
    /// <summary>
    /// Date and time of event
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
    /// Event message
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    /// Event data
    /// </summary>
    List<IAuditEventData> EventData { get; set; }
}

/// <summary>
/// Interface for audit event data
/// </summary>
public interface IAuditEventData
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