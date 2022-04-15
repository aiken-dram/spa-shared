namespace Shared.Application.Interfaces;

/// <summary>
/// Database dictionary interface
/// </summary>
public interface IDictionary
{
    /// <summary>
    /// Dictionary value
    /// </summary>
    /// <example>1</example>
    long Value { get; set; }

    /// <summary>
    /// Dictionary text
    /// </summary>
    /// <example>Test</example>
    string Text { get; set; }
}
