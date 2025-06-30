using Bookings.Domain;

namespace Bookings.Domain.Dto;

public record BaseRequestResponseDto
{
    public bool IsRequestResponsePattern { get; set; }
}

public record BookingBaseDto : BaseRequestResponseDto
{
    public string BookingId { get; set; } = string.Empty;
    public string HotelId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public double Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Adults { get; set; }
    public int Kids { get; set; }
    public string Category { get; set; } = string.Empty;
    public string StateId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public record BookingDto : BookingBaseDto
{
    // Можно добавить дополнительные поля, если потребуется
}