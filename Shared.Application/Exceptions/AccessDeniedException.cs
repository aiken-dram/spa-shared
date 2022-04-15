namespace Shared.Application.Exceptions;

/// <summary>
/// Access denied exception
/// </summary>
public class AccessDeniedException : Exception
{
    /// <summary>
    /// Access denied exception constructor
    /// </summary>
    /// <param name="message">Exception message</param>
    public AccessDeniedException(string message)
        : base(message)
    {
    }
}
