using Bookings.Domain.Commands;

namespace Bookings.Domain.Handlers;

/// <summary>
/// Интерфейс для обработчика команды создания бронирования
/// </summary>
public interface ICreateBookingCommandHandler : ICommandHandler<CreateBookingCommand, Booking>
{
} 