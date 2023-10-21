using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Bus.Sagas.Events.Realizations;
using Bookings.Domain.DTO;
using Bookings.Domain.DTO.BookingProcess;

using MassTransit;

namespace Bookings.Bus.Processors.Strategies
{
    public class BookingRequestedStrategy : IBookingStateProcessorStrategy
    {
        private readonly IBus bus;
        private readonly IRequestClient<IBookingRequested> bookingRequestedEventClient;

        public BookingRequestedStrategy(
            IBus bus,
            IRequestClient<IBookingRequested> bookingRequestedEventClient)
        {
            this.bus = bus;
            this.bookingRequestedEventClient = bookingRequestedEventClient;
        }

        public async Task<Response<BookingProcessDto>?> Execute(BookingDTO bookingModel)
        {
            // Отправка запроса на бронирование
            var bookingRequestedMessage = new BookingRequested
            {
                CorrelationId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,

                // Дополнительные свойства для запроса бронирования
                BookName = bookingModel.BookName,
                Category = bookingModel.Category,
                HotelId = bookingModel.HotelId,
                Price = bookingModel.Price,
            };

            if (!bookingModel.IsRequestResponsePattern)
            {
                await bus.Publish<IBookingRequested>(bookingRequestedMessage);
                return null;
            }

            var result = await bookingRequestedEventClient
                .GetResponse<BookingProcessDto>(bookingRequestedMessage);
            return result;
        }
    }
}
