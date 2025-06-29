namespace Bookings.Domain;

/// <summary>
/// Доменная сущность номера
/// </summary>
public record Room : BaseObject
{
    public string Name { get; init; } = string.Empty;
    public int MaxPersons { get; init; }
    public int AdditionalFreeKids { get; init; }
    public Hotel Hotel { get; init; } = null!;

    public Room() : base() { }

    public Room(
        string name,
        int maxPersons,
        int additionalFreeKids,
        Hotel hotel) : base()
    {
        Name = name;
        MaxPersons = maxPersons;
        AdditionalFreeKids = additionalFreeKids;
        Hotel = hotel;
    }

    /// <summary>
    /// Создает новый номер с обновленными данными
    /// </summary>
    public Room WithUpdatedData(
        string? name = null,
        int? maxPersons = null,
        int? additionalFreeKids = null,
        Hotel? hotel = null)
    {
        return this with
        {
            Name = name ?? Name,
            MaxPersons = maxPersons ?? MaxPersons,
            AdditionalFreeKids = additionalFreeKids ?? AdditionalFreeKids,
            Hotel = hotel ?? Hotel
        };
    }

    /// <summary>
    /// Проверяет, подходит ли номер для указанного количества людей
    /// </summary>
    public bool CanAccommodate(int adults, int kids = 0)
    {
        if (adults <= 0) return false;
        if (kids < 0) return false;
        
        var totalPeople = adults + kids;
        var freeKids = Math.Min(kids, AdditionalFreeKids);
        var paidPeople = adults + (kids - freeKids);
        
        return paidPeople <= MaxPersons;
    }

    /// <summary>
    /// Возвращает максимальное количество детей, которые могут разместиться бесплатно
    /// </summary>
    public int GetMaxFreeKids() => AdditionalFreeKids;

    /// <summary>
    /// Возвращает полное название номера с отелем
    /// </summary>
    public string GetFullName() => $"{Hotel.Name} - {Name}";

    /// <summary>
    /// Проверяет, является ли номер семейным
    /// </summary>
    public bool IsFamilyRoom => MaxPersons >= 4 && AdditionalFreeKids > 0;
}
