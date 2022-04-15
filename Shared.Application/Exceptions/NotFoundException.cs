using System;

namespace Shared.Application.Exceptions;

/// <summary>
/// Not found exception
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Not found exception constructor
    /// </summary>
    public NotFoundException()
        : base()
    {
    }

    /// <summary>
    /// Not found exception constructor
    /// </summary>
    /// <param name="message">Error message</param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Not found exception constructor
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Entity not found exception constructor
    /// </summary>
    /// <param name="name">Name of entity</param>
    /// <param name="key">Identity</param>
    public NotFoundException(string name, object key)
        : base(Messages.NotFoundException(name, key))
    {
    }
}
