using Bookings.Domain.DTO.BookingProcess;

namespace Bookings.Domain.DTO;

public record BaseRequestResponseDTO
{
    public bool IsRequestResponsePattern { get; set; }
}

public record BookingDTO : BaseRequestResponseDTO
{
    public double Price { get; set; }

    public string? BookName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Category { get; set; }

    public required string HotelId { get; set; }
}

public record UpdateBookingDTO : BookingDTO
{
    public Guid CorrelationId { get; set; }

    public string BookingId { get; set; }

    public BookingState State { get; set; }
}