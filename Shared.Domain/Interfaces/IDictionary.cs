namespace Shared.Domain.Interfaces;

/// <summary>
/// Dictionary type interface
/// </summary>
public interface IDictionaryType
{
    /// <summary>
    /// Identity of type
    /// </summary>
    long IdType { get; set; }

    /// <summary>
    /// Type name
    /// </summary>
    string Type { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    string Description { get; set; }
}

/// <summary>
/// Dictionary state interface
/// </summary>
public interface IDictionaryState
{
    /// <summary>
    /// Identity of state
    /// </summary>
    long IdState { get; set; }

    /// <summary>
    /// State name
    /// </summary>
    string State { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    string Description { get; set; }
}
