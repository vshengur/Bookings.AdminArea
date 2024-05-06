namespace Bookings.Domain;

public abstract class BaseObject : IBaseObject
{
    public string Id { get; private set; }

    public DateTime Created { get; private set; }

    public DateTime? EditedAt { get; private set; }

    public BaseObject()
    {
        Created = DateTime.UtcNow;
    }

    public BaseObject(string id, DateTime created)
    {
        Id = id;
        Created = created;
    }

    public void Updated()
    {
        EditedAt = DateTime.UtcNow;
    }
}