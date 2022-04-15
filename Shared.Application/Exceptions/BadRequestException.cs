namespace Shared.Application.Exceptions;

/// <summary>
/// Bad request exception
/// </summary>
public class BadRequestException : Exception
{
    /// <summary>
    /// Bad request exception constructor
    /// </summary>
    /// <param name="message">Exception message</param>
    public BadRequestException(string message)
        : base(message)
    {
    }
}
