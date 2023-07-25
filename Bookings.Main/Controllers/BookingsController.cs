namespace Bookings.Web.Controllers
{
    using Bookings.Domain.DTO;
    using Bookings.Domain.Queues.Messages;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingsController"/> class.
        /// </summary>
        /// <param name="client">GRPC client.</param>
        /// <param name="logger">Some logger.</param>
        /// <param name="bus">Bus abstraction.</param>
        public BookingsController(
            BookingsContract.BookingsContractClient client,
            ILogger<BookingsController> logger,
            IBus bus)
        {
            this.client = client;
            this.logger = logger;
            this.bus = bus;
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
        public async Task PostAsync([FromBody] BookingDTO bookingModel)
        {
            var newItem = new CreateBookingMessage()
            {
                BookName = bookingModel.BookName,
                CreatedDate = DateTime.Now,
                Category = bookingModel.Category,
                HotelId = bookingModel.HotelId,
                Price = bookingModel.Price,
            };

            await this.bus.Publish(newItem).ConfigureAwait(false);
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