namespace Bookings.Domain;

/// <summary>
/// Доменная сущность отеля
/// </summary>
public record Hotel : BaseObject
{
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int Stars { get; init; }
    public int Rate { get; init; }
    public double LocationX { get; init; }
    public double LocationY { get; init; }

    public Hotel() : base() { }

    public Hotel(string id, DateTime created) : base(id, created) { }

    public Hotel(
        string name,
        string city,
        string country,
        int stars,
        int rate,
        double locationX,
        double locationY) : base()
    {
        Name = name;
        City = city;
        Country = country;
        Stars = stars;
        Rate = rate;
        LocationX = locationX;
        LocationY = locationY;
    }

    /// <summary>
    /// Создает новый отель с обновленными данными
    /// </summary>
    public Hotel WithUpdatedData(
        string? name = null,
        string? city = null,
        string? country = null,
        int? stars = null,
        int? rate = null,
        double? locationX = null,
        double? locationY = null)
    {
        return this with
        {
            Name = name ?? Name,
            City = city ?? City,
            Country = country ?? Country,
            Stars = stars ?? Stars,
            Rate = rate ?? Rate,
            LocationX = locationX ?? LocationX,
            LocationY = locationY ?? LocationY
        };
    }

    /// <summary>
    /// Проверяет, является ли отель премиум-классом
    /// </summary>
    public bool IsPremium => Stars >= 4 && Rate >= 8;

    /// <summary>
    /// Возвращает полный адрес отеля
    /// </summary>
    public string GetFullAddress() => $"{Name}, {City}, {Country}";

    /// <summary>
    /// Проверяет, находится ли отель в указанном городе
    /// </summary>
    public bool IsInCity(string city) => 
        !string.IsNullOrWhiteSpace(city) && 
        City.Equals(city, StringComparison.OrdinalIgnoreCase);
}