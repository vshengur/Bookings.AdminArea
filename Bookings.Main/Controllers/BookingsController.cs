namespace Bookings.Web.Controllers
{
    using Bookings.Bus.Queues.Messages;
    using Bookings.Bus.Sagas.Events.Abstractions;
    using Bookings.Bus.Sagas.Events.Realizations;
    using Bookings.Domain.DTO;
    using Bookings.Domain.DTO.BookingProcess;
    using Bookings.Web.Models.Responses;
    using Grpc.Core;
    using MassTransit;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер упралвения заказами.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingsContract.BookingsContractClient client;
        private readonly ILogger<BookingsController> logger;
        private readonly IBus bus;
        private readonly IRequestClient<IBookingRequested> bookingRequestedEventClient;
        private readonly IRequestClient<IBookingCancelled> bookingCancelledEventClient;
        private readonly IRequestClient<IBookingConfirmed> bookingConfirmedEventClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingsController"/> class.
        /// </summary>
        /// <param name="client">GRPC client.</param>
        /// <param name="logger">Some logger.</param>
        /// <param name="bus">Bus abstraction.</param>
        /// <param name="bookingRequestedEventClient">Client for event <see cref="IBookingRequested"/>.</param>
        /// <param name="bookingCancelledEventClient">Client for event <see cref="IBookingRequested"/>.</param>
        /// <param name="bookingConfirmedEventClient">Client for event <see cref="IBookingRequested"/>.</param>
        public BookingsController(
            BookingsContract.BookingsContractClient client,
            ILogger<BookingsController> logger,
            IBus bus,
            IRequestClient<IBookingRequested> bookingRequestedEventClient,
            IRequestClient<IBookingCancelled> bookingCancelledEventClient,
            IRequestClient<IBookingConfirmed> bookingConfirmedEventClient)
        {
            this.client = client;
            this.logger = logger;
            this.bus = bus;
            this.bookingRequestedEventClient = bookingRequestedEventClient;
            this.bookingCancelledEventClient = bookingCancelledEventClient;
            this.bookingConfirmedEventClient = bookingConfirmedEventClient;
        }

        /// <summary>
        /// Получить список заказаов.
        /// </summary>
        /// <returns>Список заказов.</returns>
        [HttpGet]
        public async Task<RestApiResponse<BookingsResponse>> GetAsync()
        {
            try
            {
                var bookings = await this.client.GetBookingsAsync(
                    new BookingsRequest
                    {
                        Count = 30,
                        Page = 0,
                    },
                    deadline: DateTime.UtcNow.AddSeconds(10));

                return RestApiResponse<BookingsResponse>.Success(bookings);
            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded)
            {
                // Обработаем таймаут операции
                return RestApiResponse<BookingsResponse>.NonSuccess(ex.Message);
            }
        }

        /// <summary>
        /// Создать новую запись о бронировании.
        /// </summary>
        /// <param name="bookingModel">Модель нового бронирования.</param>
        /// <returns>Результат оформления операции.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookingDTO bookingModel)
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
                return new NoContentResult();
            }

            var result = await bookingRequestedEventClient
                .GetResponse<BookingProcessDto>(bookingRequestedMessage);

            return new JsonResult(result);
        }

        /// <summary>
        /// Создать новую запись о бронировании.
        /// </summary>
        /// <param name="bookingModel">Модель нового бронирования.</param>
        /// <returns>Результат оформления операции.</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateBookingDTO bookingModel)
        {
            switch (bookingModel.State)
            {
                case BookingState.Confirmed:
                    {
                        // Отправка запроса на бронирование
                        var bookingConfirmedMessage = new BookingConfirmed
                        {
                            CorrelationId = bookingModel.CorrelationId,
                            Timestamp = DateTime.UtcNow,

                            // Дополнительные свойства для запроса бронирования
                            BookingId = bookingModel.BookingId,
                            BookName = bookingModel.BookName,
                            Category = bookingModel.Category,
                            HotelId = bookingModel.HotelId,
                            Price = bookingModel.Price,
                        };

                        if (!bookingModel.IsRequestResponsePattern)
                        {
                            await bus.Publish<IBookingConfirmed>(bookingConfirmedMessage);
                            return new NoContentResult();
                        }

                        var result = await bookingConfirmedEventClient
                            .GetResponse<BookingProcessDto>(bookingConfirmedMessage);
                        return new JsonResult(result);
                    }

                case BookingState.Cancelled:
                    {
                        // Отправка запроса на бронирование
                        var bookingCancelledMessage = new BookingCancelled
                        {
                            CorrelationId = bookingModel.CorrelationId,
                            Timestamp = DateTime.UtcNow,

                            // Дополнительные свойства для запроса бронирования
                            BookingId = bookingModel.BookingId,
                            BookName = bookingModel.BookName,
                            Category = bookingModel.Category,
                            HotelId = bookingModel.HotelId,
                            Price = bookingModel.Price,
                        };

                        if (!bookingModel.IsRequestResponsePattern)
                        {
                            await bus.Publish<IBookingCancelled>(bookingCancelledMessage);
                            return new NoContentResult();
                        }

                        var result = await bookingCancelledEventClient
                            .GetResponse<BookingProcessDto>(bookingCancelledMessage);
                        return new JsonResult(result);
                    }

                default:
                    return new NoContentResult();
            }
        }

        /// <summary>
        /// Обновить запись о бронировании.
        /// </summary>
        /// <param name="id">Идентификатор бронирования.</param>
        /// <param name="bookingModel">Модель нового бронирования.</param>
        /// <returns>Результат оформления операции.</returns>
        [HttpPut]
        [Route("api/[controller]/{id}")]
        public async Task PutAsync(string id, [FromBody] BookingDTO bookingModel)
        {
            var newItem = new UpdateBookingMessage()
            {
                BookingId = id,
                BookName = bookingModel.BookName,
                Category = bookingModel.Category,
                Price = bookingModel.Price,
            };

            await this.bus.Send(newItem).ConfigureAwait(false);
        }
    }
}