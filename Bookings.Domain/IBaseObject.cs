namespace Bookings.Domain;

public interface IBaseObject
{
    string Id { get; }

    DateTime Created { get; }

    DateTime? EditedAt { get; }
}
