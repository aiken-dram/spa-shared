using System.Data.Common;
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
    /// Fixing DbCommand for old IBM DB2 database
    /// </summary>
    /// <param name="command">Db2Command</param>
    private static void ManipulateCommand(DbCommand command)
    {
        if (command.CommandText.StartsWith("-- FIRST PAGE TABLE", StringComparison.Ordinal))
        {
            int fetch = 10;
            //should prolly use like regex here but mendokusai
            if (command.CommandText.StartsWith("-- FIRST PAGE TABLE N50", StringComparison.Ordinal))
                fetch = 50;
            if (command.CommandText.StartsWith("-- FIRST PAGE TABLE N100", StringComparison.Ordinal))
                fetch = 100;
            if (command.CommandText.StartsWith("-- FIRST PAGE TABLE N60000", StringComparison.Ordinal))
                fetch = 60000;
            FixFetchFirst(ref command, fetch);
        }

        //fix CAST 1 AS SMALLINT to boolean error in DB9.8
        /*if(command.CommandText.Contains("ELSE CAST(1 AS SMALLINT) END")
        //    command.CommandText.Replace("", "")/**/
        //i think it was fixed in some of recent release of DB2 Entity framework package, since couldnt replicate
    }
}
