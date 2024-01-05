using Bookings.Bus.Processors.Strategies;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Bus.Processors;

/// <summary>
/// Процессор для орбработки бронирований.
/// </summary>
public class BookingStateProcessor
{
    /// <summary>
    /// Процессор хранит ссылку на один из объектов Стратегии. Процессор не
    /// знает конкретного класса стратегии. Он должен работать со всеми
    /// стратегиями через интерфейс Стратегии.
    /// </summary>
    private IBookingStateProcessorStrategy _strategy;

    public BookingStateProcessor()
    { }

    /// <summary>
    /// Процессор принимает стратегию через конструктор, а также
    /// предоставляет сеттер для её изменения во время выполнения.
    /// </summary>
    /// <param name="strategy"></param>
    public BookingStateProcessor(IBookingStateProcessorStrategy strategy)
    {
        _strategy = strategy;
    }

    /// <summary>
    /// Процессор позволяет заменить объект Стратегии во время выполнения.
    /// </summary>
    /// <param name="strategy"></param>
    public void SetStrategy(IBookingStateProcessorStrategy strategy)
    {
        _strategy = strategy;
    }

    /// <summary>
    /// Вместо того, чтобы самостоятельно реализовывать множественные версии
    /// алгоритма, Процессор делегирует некоторую работу объекту Стратегии.
    /// </summary>
    /// <param name="bookingDTO"></param>
    /// <returns></returns>
    public async Task<Response<BookingProcessDto>?> Proceed(BookingDto bookingDTO)
    {
        return await _strategy.Execute(bookingDTO);
    }
}
