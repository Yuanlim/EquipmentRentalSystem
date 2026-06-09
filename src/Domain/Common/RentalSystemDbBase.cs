using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalSystem.Domain.Common;

public abstract class RentalSystemDbBase
{
    [Required]
    public Guid Id { get; init; }

    // This cant be reassign new list
    private readonly List<BaseEvent> _domainEvents = [];

    [NotMapped]
    // Reader -> cant be mutate
    public IReadOnlyCollection<BaseEvent> DomainEvent => _domainEvents.AsReadOnly();

    public void AddEvent(BaseEvent baseEvent)
    {
        _domainEvents.Add(baseEvent);
    }

    public void RemoveEvent(BaseEvent baseEvent)
    {
        _domainEvents.Remove(baseEvent);
    }

    public void ClearEvent()
    {
        _domainEvents.Clear();
    }

    protected static string ValidateUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cant be null or whitespace");

        return userId;
    }
}