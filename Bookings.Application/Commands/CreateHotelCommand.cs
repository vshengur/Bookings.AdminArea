namespace Bookings.Application.Commands;

/// <summary>
/// Команда для создания отеля
/// </summary>
public record CreateHotelCommand
{
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int Stars { get; init; }
    public int Rate { get; init; }
    public double LocationX { get; init; }
    public double LocationY { get; init; }
} 