using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shared.Infrastructure.Interceptors;

/// <summary>
/// Entity framework DbCommand interceptor for fixing queries for old IBM DB2 database
/// </summary>
public class DB9QueryInterceptor : DbCommandInterceptor
{
    /// <summary>
    /// Intercepting reading result from Db2Command
    /// </summary>
    /// <param name="command">Db2Command</param>
    /// <param name="eventData">CommandEventData</param>
    /// <param name="result">Result</param>
    /// <returns></returns>
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ManipulateCommand(command);

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        ManipulateCommand(command);

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    /// <summary>
    /// Fixing FETCH FIRST queries
    /// </summary>
    /// <param name="command">Db2Command</param>
    /// <param name="Rows">Number of retrieving rows in FETCH FIRST</param>
    private static void FixFetchFirst(ref DbCommand command, int Rows)
    {
        if (command.CommandText.Contains("FETCH FIRST"))
        {
            int i = command.CommandText.LastIndexOf("FETCH FIRST");
            int j = command.CommandText.LastIndexOf("ROWS ONLY");
            string cmd0 = command.CommandText.Substring(0, i);
            string fetch = $"FETCH FIRST {Rows} ";
            string cmd1 = command.CommandText.Substring(j);
            command.CommandText = cmd0 + fetch + cmd1;
        }
    }

    /// <summary>
    /// Find N in FETCH FIRST N ROWS
    /// </summary>
    /// <param name="cmd">Db2Command</param>
    /// <returns>Number of retrieving rows in FETCH FIRST</returns>
    private static int GetFetchFirst(string cmd)
    {
        var regex = @"^-- TABLE FIRST PAGE N(\d+)ROWS";
        Regex rFetch = new Regex(regex, RegexOptions.IgnoreCase);
        var matches = rFetch.Matches(cmd);
        var rows = matches[0].Groups[1].Value;
        return Convert.ToInt32(rows);
    }

    /// <summary>
    /// Fixing DbCommand for old IBM DB2 database
    /// </summary>
    /// <param name="command">Db2Command</param>
    private static void ManipulateCommand(DbCommand command)
    {
        //fix FETCH FIRST ROWS ONLY error in DB9.8
        var search = "-- TABLE FIRST PAGE N";
        if (command.CommandText.StartsWith(search, StringComparison.Ordinal))
            FixFetchFirst(ref command, GetFetchFirst(command.CommandText));

        //fix CAST 1 AS SMALLINT to boolean error in DB9.8
        search = "-- TABLE";
        if (command.CommandText.StartsWith(search, StringComparison.Ordinal))
        {
            search = "THEN CAST(1 AS smallint)\r\n    ELSE CAST(0 AS smallint)\r\nEND";
            if (command.CommandText.Contains(search))
            {
                var txt = command.CommandText;
                var fix = txt.Replace(search, search + " = CAST(1 AS smallint)");
                command.CommandText = fix;
            }
        }
    }
}
