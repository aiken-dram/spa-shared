using FluentValidation.Results;

namespace Shared.Application.Exceptions;

/// <summary>
/// Validation exception
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Validation exception constructor
    /// </summary>
    public ValidationException()
        : base(Messages.ValidationException)
    {
        Failures = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Validation exception constructor
    /// </summary>
    /// <param name="failures">List of validation failures</param>
    public ValidationException(List<ValidationFailure> failures)
        : this()
    {
        var propertyNames = failures
            .Select(e => e.PropertyName)
            .Distinct();

        foreach (var propertyName in propertyNames)
        {
            var propertyFailures = failures
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    /// <summary>
    /// Dictionary of validation failures
    /// </summary>
    public IDictionary<string, string[]> Failures { get; }
}
