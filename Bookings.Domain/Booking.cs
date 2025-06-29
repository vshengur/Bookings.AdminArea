namespace Bookings.Domain;

/// <summary>
/// Доменная сущность бронирования
/// </summary>
public record Booking : BaseObject
{
    public string BookName { get; init; } = string.Empty;
    public Room Room { get; init; } = null!;
    public double Price { get; init; }
    public string Category { get; init; } = string.Empty;
    public Guid StateId { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int Adults { get; init; }
    public int Kids { get; init; }

    public Booking() : base() { }

    public Booking(
        string bookName, 
        Room room, 
        double price, 
        string category, 
        Guid stateId, 
        DateOnly startDate, 
        DateOnly endDate, 
        int adults, 
        int kids) : base()
    {
        BookName = bookName;
        Room = room;
        Price = price;
        Category = category;
        StateId = stateId;
        StartDate = startDate;
        EndDate = endDate;
        Adults = adults;
        Kids = kids;
    }

    /// <summary>
    /// Создает новое бронирование с обновленными данными
    /// </summary>
    public Booking WithUpdatedData(
        string? bookName = null,
        Room? room = null,
        double? price = null,
        string? category = null,
        Guid? stateId = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        int? adults = null,
        int? kids = null)
    {
        return this with
        {
            BookName = bookName ?? BookName,
            Room = room ?? Room,
            Price = price ?? Price,
            Category = category ?? Category,
            StateId = stateId ?? StateId,
            StartDate = startDate ?? StartDate,
            EndDate = endDate ?? EndDate,
            Adults = adults ?? Adults,
            Kids = kids ?? Kids
        };
    }

    /// <summary>
    /// Проверяет, активна ли бронь
    /// </summary>
    public bool IsActive => StartDate <= DateOnly.FromDateTime(DateTime.Today) && EndDate >= DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// Возвращает общую стоимость бронирования
    /// </summary>
    public double GetTotalPrice() => Price * GetTotalDays();

    /// <summary>
    /// Возвращает количество дней бронирования
    /// </summary>
    public int GetTotalDays() => EndDate.DayNumber - StartDate.DayNumber;
}