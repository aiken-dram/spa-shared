namespace Shared.Domain.Models;

/// <summary>
/// Entity is auditable and has list of audit events to be stored in database
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// List of audit to be logged in database
    /// </summary>
    public List<Audit> Audits { get; set; } = new List<Audit>();

    /// <summary>
    /// Add audit to list
    /// </summary>
    /// <param name="e">Audit</param>
    public void Log(Audit e) => Audits.Add(e);

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
