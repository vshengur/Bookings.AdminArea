using Bookings.Application.Commands;
using Bookings.Domain;

namespace Bookings.Application.Handlers;

/// <summary>
/// Интерфейс для обработчика команды создания бронирования
/// </summary>
public interface ICreateBookingCommandHandler : ICommandHandler<CreateBookingCommand, Booking>
{
} 