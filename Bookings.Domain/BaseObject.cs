namespace Bookings.Domain;

public abstract class BaseObject : IBaseObject
{
    public Guid Id { get; private set; }

    public DateTime Created { get; private set; }

    public DateTime? EditedAt { get; private set; }

    public BaseObject()
    {
        Created = DateTime.UtcNow;
    }

    public void Updated()
    {
        EditedAt = DateTime.UtcNow;
    }
}