namespace Bookings.Domain;

/// <summary>
/// Базовый доменный объект, следующий принципам DDD и чистой архитектуры
/// </summary>
public abstract record BaseObject : IBaseObject
{
    public string Id { get; init; }
    public DateTime Created { get; init; }
    public DateTime? EditedAt { get; private set; }

    protected BaseObject()
    {
        Id = Guid.NewGuid().ToString();
        Created = DateTime.UtcNow;
    }

    protected BaseObject(string id, DateTime created)
    {
        Id = id;
        Created = created;
    }

    /// <summary>
    /// Отмечает объект как обновленный
    /// </summary>
    public void MarkAsUpdated()
    {
        EditedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Создает копию объекта с обновленным временем редактирования
    /// </summary>
    protected BaseObject WithUpdated()
    {
        var copy = this with { };
        copy.EditedAt = DateTime.UtcNow;
        return copy;
    }
}