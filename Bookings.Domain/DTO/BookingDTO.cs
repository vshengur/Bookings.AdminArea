using Bookings.Domain.Dto.BookingProcess;

namespace Bookings.Domain.Dto;

public record BaseRequestResponseDto
{
    public bool IsRequestResponsePattern { get; set; }
}

public record BookingBaseDto : BaseRequestResponseDto
{
    public string RoomId { get; set; }

    public string? BookingId { get; set; }

    public BookingState? State { get; set; }
}

public record BookingDto : BookingBaseDto
{
    public double Price { get; set; }

    public string? BookName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Category { get; set; }
}