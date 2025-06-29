using Bookings.Application.Commands;
using Bookings.Application.Handlers;
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
        _logger.LogInformation("Создание бронирования: {BookName} для номера {RoomId}", 
            command.BookName, command.RoomId);

        // Здесь будет логика создания бронирования, например, получение номера по ID
        // Пока что создаем временный номер для примера
        var hotel = new Hotel("temp-hotel", DateTime.UtcNow);
        var room = new Room("temp-room", 2, 1, hotel);
        
        var booking = new Booking(
            command.BookName,
            room,
            command.Price,
            command.Category,
            command.StateId,
            command.StartDate,
            command.EndDate,
            command.Adults,
            command.Kids
        );

        _logger.LogInformation("Бронирование создано успешно с ID: {BookingId}", booking.Id);

        return await Task.FromResult(booking);
    }
} 