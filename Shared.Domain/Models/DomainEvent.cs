namespace Shared.Domain.Models;

/// <summary>
/// Interface for entities that have domain events
/// </summary>
public interface IHasDomainEvent
{
    /// <summary>
    /// List of domain events in entity
    /// </summary>
    public List<DomainEvent> DomainEvents { get; set; }
}

/// <summary>
/// Abstract domain event
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Default constructor
    /// </summary>
    protected DomainEvent()
    {
        DateOccurred = DateTime.Now;
    }

    /// <summary>
    /// Has been published in MediatR
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Date and time of event occurrance
    /// </summary>
    public DateTime DateOccurred { get; protected set; } = DateTime.Now;
}
