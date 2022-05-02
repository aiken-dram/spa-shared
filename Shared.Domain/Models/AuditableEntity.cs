namespace Shared.Domain.Models;

/// <summary>
/// Entity is auditable and has list of audit events to be stored in database
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// Events to be logged in database
    /// </summary>
    public List<AuditEvent> AuditEvents { get; set; } = new List<AuditEvent>();

    /// <summary>
    /// Add audit event to event log
    /// </summary>
    /// <param name="e"></param>
    public void Log(AuditEvent e) => AuditEvents.Add(e);

    /// <summary>
    /// Id of target in dictionary to be saved in AuditEvent
    /// </summary>
    public abstract int AuditIdTarget { get; }

    /// <summary>
    /// Id of target entity to be saved in AuditEvent
    /// </summary>
    public abstract long? AuditTargetId { get; }

    /// <summary>
    /// Name of target to be saved in AuditEvent
    /// </summary>
    public abstract string AuditTargetName { get; }
}
