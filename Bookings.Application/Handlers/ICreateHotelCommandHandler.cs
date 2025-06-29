using Bookings.Application.Commands;
using Bookings.Domain;

namespace Bookings.Application.Handlers;

/// <summary>
/// Интерфейс для обработчика команды создания отеля
/// </summary>
public interface ICreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, Hotel>
{
} 