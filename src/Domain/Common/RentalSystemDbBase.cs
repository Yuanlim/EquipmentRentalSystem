using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace RentalSystem.Domain.Common;

public abstract class RentalSystemDbBase
{
    [Required]
    public Guid Id { get; init; } = Guid.NewGuid();

    // This cant be reassign new list
    private readonly List<BaseEvent> _domainEvents = [];

    [NotMapped]
    // Reader -> cant be mutate
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent baseEvent)
    {
        _domainEvents.Add(baseEvent);
    }

    public void RemoveDomainEvent(BaseEvent baseEvent)
    {
        _domainEvents.Remove(baseEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    // Purpose: FluentValidation will be by pass in domain test

    public static void ThrowIfIsNullOrWhiteSpace<TClass>(
        string? text,
        [CallerArgumentExpression(nameof(text))] string? fieldName = null
    )
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException($"{typeof(TClass).Name} {SplitFieldName(fieldName)} shouldn't be whitespace, empty or null.");
    }

    public static void ThrowIfIsNull<TClass, TData>(
        TData? data,
        [CallerArgumentExpression(nameof(data))] string? fieldName = null
    )
    {
        if (data is null)
            throw new ArgumentNullException($"{typeof(TClass).Name} {SplitFieldName(fieldName)} shouldn't be null.");
    }

    public static void ThrowIfExceededLength<TClass>(
        string? data,
        int length,
        [CallerArgumentExpression(nameof(data))] string? fieldName = null
    )
    {
        if (!string.IsNullOrWhiteSpace(data) && data.Length > length)
            throw new ArgumentException($"{typeof(TClass).Name} {SplitFieldName(fieldName)} length shouldn't be larger than {length}.");
    }

    public static void ThrowIfBelowLength<TClass>(
        string? data,
        int length,
        [CallerArgumentExpression(nameof(data))] string? fieldName = null
    )
    {
        if (data is null || data.Length < length)
            throw new ArgumentException($"{typeof(TClass).Name} {SplitFieldName(fieldName)} length shouldn't be lower than {length} or null.");
    }

    public static void ThrowIfLessThan<TClass>(
        decimal data,
        decimal threshold,
        [CallerArgumentExpression(nameof(data))] string? fieldName = null
    )
    {
        if (data < threshold)
            throw new ArgumentException($"{typeof(TClass).Name} {SplitFieldName(fieldName)} value should be greater than {threshold}.");
    }

    public static void ThrowIfGreaterThan<TClass>(
        decimal data,
        decimal threshold,
        [CallerArgumentExpression(nameof(data))] string? fieldName = null
    )
    {
        if (data > threshold)
            throw new ArgumentException($"{typeof(TClass).Name} {SplitFieldName(fieldName)} value should be less than {threshold}.");
    }

    protected static string ValidateUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cant be null or whitespace");

        return userId;
    }

    private static string SplitFieldName(string? fieldName)
    {
        // string with "+" arithmetic operator creates new string every time because it is immutable
        // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#using-stringbuilder-for-fast-string-creation
        // with StringBuilder you can mutate string in a faster way

        if (string.IsNullOrWhiteSpace(fieldName))
            return "unknown field name";

        var split = new StringBuilder();

        for (int i = 0; i < fieldName.Length; i++)
        {
            var c = fieldName[i];

            if (i > 0 && char.IsUpper(c))
                split.Append(' ');

            split.Append(char.ToLowerInvariant(c));
        }

        return split.ToString();
    }
}