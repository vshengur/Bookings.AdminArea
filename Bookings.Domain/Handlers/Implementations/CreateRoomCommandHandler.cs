using Bookings.Domain.Commands;
using Microsoft.Extensions.Logging;

namespace Bookings.Domain.Handlers.Implementations;

/// <summary>
/// Реализация обработчика команды создания номера
/// </summary>
public class CreateRoomCommandHandler : ICreateRoomCommandHandler
{
    private readonly ILogger<CreateRoomCommandHandler> _logger;

    public CreateRoomCommandHandler(ILogger<CreateRoomCommandHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Room> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Создание номера: {RoomName} для {MaxPersons} человек", 
            command.Name, command.MaxPersons);

        // Здесь будет логика создания номера, например, получение отеля по ID
        // Пока что создаем временный отель для примера
        var hotel = new Hotel("temp-hotel", DateTime.UtcNow);
        
        var room = new Room(
            command.Name,
            command.MaxPersons,
            command.AdditionalFreeKids,
            hotel
        );

        _logger.LogInformation("Номер создан успешно с ID: {RoomId}", room.Id);

        return await Task.FromResult(room);
    }
} 