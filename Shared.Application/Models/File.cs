namespace Shared.Application.Models;

/// <summary>
/// Static class for file content types
/// </summary>
public static class FileContentType
{
    /// <summary>
    /// Comma separated value content type
    /// </summary>
    public const string CSV = "text/csv";

    /*
    2D : add more file content types
    */
}

/// <summary>
/// View model for file content
/// </summary>
public class FileVm
{
    /// <summary>
    /// Name of file
    /// </summary>
    /// <example>export.csv</example>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// Type of file
    /// </summary>
    /// <example>text/csv</example>
    public string ContentType { get; set; } = null!;

    /// <summary>
    /// Byte content of file
    /// </summary>
    public byte[] Content { get; set; } = null!;
}
