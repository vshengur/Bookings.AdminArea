namespace Bookings.AvailableRooms.Models.Options;

internal record TransientFaultHandlingOptions
{
    public bool Enabled { get; set; }
    public TimeSpan AutoRetryDelay { get; set; }
}
