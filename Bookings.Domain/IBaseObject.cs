namespace Bookings.Domain;

public interface IBaseObject
{
    Guid Id { get; }

    DateTime Created { get; }

    DateTime? EditedAt { get; }
}
