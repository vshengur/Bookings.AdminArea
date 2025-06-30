namespace Bookings.Domain;

/// <summary>
/// Доменная сущность бронирования
/// </summary>
public record Booking : BaseObject
{
    public string HotelId { get; set; }
    public string RoomId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public double Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Adults { get; set; }
    public int Kids { get; set; }
    public string Category { get; set; } = string.Empty;
    public Guid StateId { get; set; }
    // Дополнительные поля по необходимости
}

public class BookingBuilder
{
    private readonly Booking _booking = new();

    public BookingBuilder SetHotelId(string hotelId) { _booking.HotelId = hotelId; return this; }
    public BookingBuilder SetRoomId(string roomId) { _booking.RoomId = roomId; return this; }
    public BookingBuilder SetGuestName(string guestName) { _booking.GuestName = guestName; return this; }
    public BookingBuilder SetGuestEmail(string guestEmail) { _booking.GuestEmail = guestEmail; return this; }
    public BookingBuilder SetCheckInDate(DateOnly checkInDate) { _booking.CheckInDate = checkInDate; return this; }
    public BookingBuilder SetCheckOutDate(DateOnly checkOutDate) { _booking.CheckOutDate = checkOutDate; return this; }
    public BookingBuilder SetCreatedAt(DateTime createdAt) { _booking.CreatedAt = createdAt; return this; }
    public BookingBuilder SetUpdatedAt(DateTime updatedAt) { _booking.UpdatedAt = updatedAt; return this; }
    public BookingBuilder SetPrice(double price) { _booking.Price = price; return this; }
    public BookingBuilder SetStatus(string status) { _booking.Status = status; return this; }
    public BookingBuilder SetAdults(int adults) { _booking.Adults = adults; return this; }
    public BookingBuilder SetKids(int kids) { _booking.Kids = kids; return this; }
    public BookingBuilder SetCategory(string category) { _booking.Category = category; return this; }
    public BookingBuilder SetStateId(Guid stateId) { _booking.StateId = stateId; return this; }

    public Booking Build() => _booking;
}