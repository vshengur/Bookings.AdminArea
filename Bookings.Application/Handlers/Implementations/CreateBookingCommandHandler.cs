using Bookings.Application.Commands;
using Bookings.Domain;

using Microsoft.Extensions.Logging;

namespace Bookings.Application.Handlers.Implementations;

/// <summary>
/// Реализация обработчика команды создания бронирования
/// </summary>
public class CreateBookingCommandHandler : ICreateBookingCommandHandler
{
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    public CreateBookingCommandHandler(ILogger<CreateBookingCommandHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Booking> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Создание бронирования: {GuestName} для номера {RoomId}", 
            command.GuestName, command.RoomId);

        // Здесь будет логика создания бронирования, например, получение номера по ID
        // Пока что создаем временный номер для примера
        var hotel = new Hotel("temp-hotel", DateTime.UtcNow);
        var room = new Room("temp-room", 2, 1, hotel);
        
        var booking = new BookingBuilder()
            .SetGuestName(command.GuestName)
            .SetRoomId(room.Id)
            .SetHotelId(hotel.Id)
            .SetPrice(command.Price)
            .SetCategory(command.Category)
            .SetStateId(command.StateId)
            .SetCheckInDate(command.StartDate)
            .SetCheckOutDate(command.EndDate)
            .SetAdults(command.Adults)
            .SetKids(command.Kids)
            .SetCreatedAt(DateTime.UtcNow)
            .SetUpdatedAt(DateTime.UtcNow)
            .Build();

        _logger.LogInformation("Бронирование создано успешно с ID: {BookingId}", booking.Id);

        return await Task.FromResult(booking);
    }
} 