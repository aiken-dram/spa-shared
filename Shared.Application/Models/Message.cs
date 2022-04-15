namespace Shared.Application.Models;

/// <summary>
/// Message
/// </summary>
public class Message
{
    /// <summary>
    /// Message state (success/error/warning/info)
    /// </summary>
    /// <example>success</example>
    public string State { get; set; }

    /// <summary>
    /// Message body
    /// </summary>
    /// <example>String has incorrect format</example>
    public string Body { get; set; }

    /// <summary>
    /// Default message
    /// </summary>
    /// <param name="body"></param>
    public Message(string body)
    {
        State = "info";
        Body = body;
    }

    /// <summary>
    /// Parametrized constructor for message
    /// </summary>
    /// <param name="state"></param>
    /// <param name="body"></param>
    public Message(string state, string body)
    {
        State = state;
        Body = body;
    }

    /// <summary>
    /// Creates new message of state 'info'
    /// </summary>
    /// <param name="body">Message body</param>
    /// <returns>Message object</returns>
    public static Message Info(string body)
    {
        return new Message("info", body);
    }

    /// <summary>
    /// Creates new message of state 'success'
    /// </summary>
    /// <param name="body">Message body</param>
    /// <returns>Message object</returns>
    public static Message Success(string body)
    {
        return new Message("success", body);
    }

    /// <summary>
    /// Creates new message of state 'warning'
    /// </summary>
    /// <param name="body">Message body</param>
    /// <returns>Message object</returns>
    public static Message Warning(string body)
    {
        return new Message("warning", body);
    }

    /// <summary>
    /// Creates new message of state 'error'
    /// </summary>
    /// <param name="body">Message body</param>
    /// <returns>Message object</returns>
    public static Message Error(string body)
    {
        return new Message("error", body);
    }
}
