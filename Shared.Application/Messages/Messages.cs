namespace Shared.Application;

/// <summary>
/// Static messages
/// </summary>
public static class Messages
{
    #region EXCEPTIONS
    /// <summary>
    /// Error while saving data to field
    /// </summary>
    public static string HistoryEditException(string field, string err) => $"Error while saving data to field '{field}': {err}";
    /// <summary>
    /// Entity was not found
    /// </summary>
    public static string NotFoundException(string entity, object id) => $"Entity \"{entity}\" ({id}) was not found.";
    /// <summary>
    /// One or more validation failures have occured
    /// </summary>
    public const string ValidationException = "One or more validation failures have occurred.";
    #endregion

    #region SHARED
    /// <summary>
    /// Text to be displayed in case of null value
    /// </summary>
    public const string NullValue = "missing";
    /// <summary>
    /// Value is hidden
    /// </summary>
    public const string HiddenValue = "(hidden)";
    /// <summary>
    /// Culture info
    /// </summary>
    public const string CultureInfo = "ru-RU";
    /// <summary>
    /// DateTime format
    /// </summary>
    public const string DateTimeFormat = "dd.MM.yyyy";
    /// <summary>
    /// Timestamp format
    /// </summary>
    public const string TimestampFormat = "dd.MM.yyyy hh:mm:ss";
    /// <summary>
    /// Currency symbol
    /// </summary>
    public const string Currency = "р.";
    /// <summary>
    /// Char boolean text for true
    /// </summary>
    public const string CharBooleanTrue = "Yes";
    /// <summary>
    /// Char boolean text for false
    /// </summary>
    public const string CharBooleanFalse = "No";
    #endregion

    #region HELPERS
    /// <summary>
    /// House
    /// </summary>
    public static string AddressHouse(string num) => $" house {num}";
    /// <summary>
    /// Block
    /// </summary>
    public static string AddressBlock(string num) => $" block {num}";
    /// <summary>
    /// Appartment
    /// </summary>
    public static string AddressAppartment(string num) => $" appartment {num}";
    /// <summary>
    /// Invalid comparison operator
    /// </summary>
    public static string InvalidComparisonOperator(string oper) => $"Invalid comparison operator '{oper}'.";
    /// <summary>
    /// Entered value into field
    /// </summary>
    public static string HistoryNewValue(string field, object val) => $"Entered value <code>{val}</code> into field <strong>{field}</strong>";
    /// <summary>
    /// Value in field replaced with
    /// </summary>
    public static string HistoryEditValue(string field, object oldVal, object newVal) => $"Value <code>{oldVal}</code> in field <strong>{field}</strong> replaced with <code>{newVal}</code>";
    #endregion

    #region MODELS
    /// <summary>
    /// Incorrect filter format
    /// </summary>
    public static string IncorrectFilterFormat(string filter) => $"Incorrect filter format: '{filter}'";

    /// <summary>
    /// Filter cannot be empty
    /// </summary>
    public const string EmptyFilter = "Filter must not be empty";
    #endregion
}