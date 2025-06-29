using Bookings.Application.Commands;
using Bookings.Domain;

namespace Bookings.Application.Handlers;

/// <summary>
/// Интерфейс для обработчика команды создания номера
/// </summary>
public interface ICreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Room>
{
} 