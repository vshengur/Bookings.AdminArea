using Bookings.Domain.Commands;

namespace Bookings.Domain.Handlers;

/// <summary>
/// Интерфейс для обработчика команды создания номера
/// </summary>
public interface ICreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Room>
{
} 