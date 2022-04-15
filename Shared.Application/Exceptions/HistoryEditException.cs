namespace Shared.Application.Exceptions;

/// <summary>
/// Error while editing entity exception
/// </summary>
public class HistoryEditException : Exception
{
    /// <summary>
    /// Error while editing entity exception constructor
    /// </summary>
    /// <param name="field">Entity field that's being edited</param>
    /// <param name="message">Error message</param>
    public HistoryEditException(string field, string message)
        : base(Messages.HistoryEditException(field, message))
    {
    }
}
