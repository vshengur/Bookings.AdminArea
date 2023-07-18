namespace Bookings.Consumers.Messages
{
    public record CreateBooking
    {
        public string? BookingId { get; init; }
        
        public string? BookName { get; init; }

        public DateTime CreatedDate { get; init; }

        public double Price { get; init; }
             
    }
}
