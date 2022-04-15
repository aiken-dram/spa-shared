namespace Shared.Application.Models;

/// <summary>
/// Class for describing result of some operation
/// </summary>
public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    /// <summary>
    /// Was operation successful
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// List of encountered errors during operation
    /// </summary>
    public string[] Errors { get; set; }

    /// <summary>
    /// Success constructor
    /// </summary>
    /// <returns>Result class</returns>
    public static Result Success()
    {
        return new Result(true, new string[] { });
    }

    /// <summary>
    /// Failure constructor with list of errors
    /// </summary>
    /// <param name="errors">List of errors</param>
    /// <returns>Result class</returns>
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }

    /// <summary>
    /// Failure constructor with single error
    /// </summary>
    /// <param name="error">Error</param>
    /// <returns>Result class</returns>
    public static Result Failure(string error)
    {
        return Result.Failure(new[] { error });
    }
}
