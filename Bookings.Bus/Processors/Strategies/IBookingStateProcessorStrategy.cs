using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Bus.Processors.Strategies;

/// <summary>
/// Интерфейс Стратегии объявляет операции, общие для 
/// всех поддерживаемых версий некоторого алгоритма.
/// Процессор использует этот интерфейс для вызова 
/// алгоритма, определённого Конкретными Стратегиями.
/// </summary>
public interface IBookingStateProcessorStrategy
{
    /// <summary>
    /// Выполнить основную логику обработки бронирования.
    /// </summary>
    /// <param name="bookingModel">Модель с данными бронирования.</param>
    /// <returns></returns>
    Task<Response<BookingProcessDto>?> Execute(BookingDto bookingModel);
}
