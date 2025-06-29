namespace Bookings.Application.Commands;

/// <summary>
/// Команда для создания номера
/// </summary>
public record CreateRoomCommand
{
    public string Name { get; init; } = string.Empty;
    public int MaxPersons { get; init; }
    public int AdditionalFreeKids { get; init; }
    public string HotelId { get; init; } = string.Empty;
} 