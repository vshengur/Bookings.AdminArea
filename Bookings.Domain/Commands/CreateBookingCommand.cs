namespace Bookings.Domain.Commands;

/// <summary>
/// Команда для создания бронирования
/// </summary>
public record CreateBookingCommand
{
    public string BookName { get; init; } = string.Empty;
    public string RoomId { get; init; } = string.Empty;
    public double Price { get; init; }
    public string Category { get; init; } = string.Empty;
    public Guid StateId { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int Adults { get; init; }
    public int Kids { get; init; }
} 