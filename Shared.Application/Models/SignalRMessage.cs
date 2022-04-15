namespace Shared.Application.Models;

/// <summary>
/// SignalR message data transfer object
/// </summary>
public class SignalRMessageDto
{
    /// <summary>
    /// From
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// To
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Body
    /// </summary>
    public object? Body { get; set; }

    /// <summary>
    /// Empty constructor
    /// </summary>
    public SignalRMessageDto()
    {

    }

    /// <summary>
    /// Parametrized constructor
    /// </summary>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    public SignalRMessageDto(string from, string to, string subject, object body)
    {
        From = from;
        To = to;
        Subject = subject;
        Body = body;
    }
}
