namespace Shared.Domain.Attributes;

/// <summary>
/// Attribute for enum dictionary
/// </summary>
public class DictionaryAttribute : Attribute
{
    /// <summary>
    /// Name of dictionary entry in database
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="name">Name of entry in database</param>
    public DictionaryAttribute(string name)
    {
        this.Name = name;
    }
}
