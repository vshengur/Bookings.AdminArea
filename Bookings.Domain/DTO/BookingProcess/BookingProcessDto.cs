namespace Bookings.Domain.DTO.BookingProcess;

public class BookingProcessDto
{
    public required string BookingId { get; set; }

    public Guid CorrelationId { get; set; }
}
