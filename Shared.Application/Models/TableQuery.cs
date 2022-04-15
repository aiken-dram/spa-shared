namespace Shared.Application.Models;

/// <summary>
/// Class for storing table filter information
/// </summary>
public class TableFilter
{
    /// <summary>
    /// Name of field
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Operator of a filter
    /// </summary>
    public string Operator { get; set; }

    /// <summary>
    /// Filtered value for a field by operator
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Construct filter from properties
    /// </summary>
    /// <param name="field">Name of field</param>
    /// <param name="oper">Name of operation</param>
    /// <param name="value">Value</param>
    public TableFilter(string field, string oper, string value)
    {
        this.Field = field;
        this.Operator = oper;
        this.Value = value;

    }

    /// <summary>
    /// Constructs TableFilter from string
    /// </summary>
    /// <param name="filter">string with filter "{fieldName}|{operator}|{value}"</param>
    public static TableFilter FromFilter(string? filter)
    {
        if (!string.IsNullOrEmpty(filter))
        {
            var s = filter.Split('|');
            if (s.Length == 3)
                return new TableFilter(s[0], s[1], s[2]);
            else
                throw new Exception(Messages.IncorrectFilterFormat(filter));
        }
        else
            throw new Exception(Messages.EmptyFilter);
    }

    /// <summary>
    /// Returns list of TableFilter from list of filter strings
    /// </summary>
    /// <param name="filters">List of filter strings</param>
    /// <returns>List of TableFilter</returns>
    public static IEnumerable<TableFilter> ToFilterList(IList<string> filters)
    {
        var res = new List<TableFilter>();
        if (filters != null && filters.Count > 0)
            foreach (var f in filters)
                res.Add(TableFilter.FromFilter(f));
        return res;
    }
}

/// <summary>
/// Query data from vuetify's datatable component with additional filter data
/// </summary>
public class TableQuery
{
    /// <summary>
    /// Number of requested page
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; }

    /// <summary>
    /// Amount of items per page
    /// </summary>
    /// <example>10</example>
    public int ItemsPerPage { get; set; }

    /// <summary>
    /// Name of field to apply ordering
    /// </summary>
    /// <example>personFamily</example>
    public string? SortBy { get; set; }

    /// <summary>
    /// Do the ordering in descending order
    /// </summary>
    /// <example>false</example>
    public bool SortDesc { get; set; }

    /// <summary>
    /// List of filters as strings with format: "{fieldName}|{operation}|{value}"
    /// </summary>
    public IList<string>? Filters { get; set; }
}

/// <summary>
/// View model for returning a list of items
/// </summary>
/// <typeparam name="T">Class of data transfer object of item</typeparam>
public class ListVm<T>
    where T : class
{
    /// <summary>
    /// List of requested items from database
    /// </summary>
    public IList<T>? Items { get; set; }
}

/// <summary>
/// View model for returning a table of items
/// </summary>
/// <typeparam name="T">Class of data transfer object of item</typeparam>
public class TableVm<T>
    where T : class
{
    /// <summary>
    /// List of requested items from database
    /// </summary>
    public IList<T>? Items { get; set; }

    /// <summary>
    /// Total count of records for request in database
    /// </summary>
    /// <example>750000</example>
    public long? Total { get; set; }
}
