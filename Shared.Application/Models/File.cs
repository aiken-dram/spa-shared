namespace Shared.Application.Models;

/// <summary>
/// Static class for file content types
/// </summary>
public static class FileContentType
{
    public const string CSV = "text/csv";

    public const string JPEG = "image/jpeg";
    public const string PNG = "image/png";

    public const string DOC = "application/msword";
    public const string DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public const string XLS = "application/vnd.ms-excel";
    public const string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public const string PPT = "application/vnd.ms-powerpoint";
    public const string PPTX = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
}

/// <summary>
/// View model for file content
/// </summary>
public class FileVm
{
    public FileVm() { }

    public FileVm(string fileName, string contentType, byte[] content)
    {
        FileName = fileName;
        ContentType = contentType;
        Content = content;
    }

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
