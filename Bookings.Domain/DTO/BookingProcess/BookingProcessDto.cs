namespace Bookings.Domain.Dto.BookingProcess;

public class BookingProcessDto
{
    public required string BookingId { get; set; }

    public Guid CorrelationId { get; set; }
}
