using Bookings.Application.Commands;
using Bookings.Application.Handlers;
using Bookings.Domain;
using Microsoft.Extensions.Logging;

namespace Bookings.Application.Handlers.Implementations;

/// <summary>
/// Реализация обработчика команды создания отеля
/// </summary>
public class CreateHotelCommandHandler : ICreateHotelCommandHandler
{
    private readonly ILogger<CreateHotelCommandHandler> _logger;

    public CreateHotelCommandHandler(ILogger<CreateHotelCommandHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Hotel> HandleAsync(CreateHotelCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Создание отеля: {HotelName} в {City}, {Country}", 
            command.Name, command.City, command.Country);

        // Здесь будет логика создания отеля, например, сохранение в базу данных
        // Пока что просто создаем доменный объект
        var hotel = new Hotel(
            command.Name,
            command.City,
            command.Country,
            command.Stars,
            command.Rate,
            command.LocationX,
            command.LocationY
        );

        _logger.LogInformation("Отель создан успешно с ID: {HotelId}", hotel.Id);

        return await Task.FromResult(hotel);
    }
} 