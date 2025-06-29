using Bookings.Domain.Commands;

namespace Bookings.Domain.Handlers;

/// <summary>
/// Интерфейс для обработчика команды создания отеля
/// </summary>
public interface ICreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, Hotel>
{
} 