namespace RentalSystem.Application.Common.Models;

/// <summary>
/// Represents whether an application request completed successfully or failed with errors.
/// </summary>
public class Result
{
    public bool Succeeded { get; init; } // If the request succeed

    public string[] Errors { get; init; } // Stores all errors that happened during the request

    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = [.. errors];
    }

    public static Result Success()
    {
        return new Result(true, []);
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}