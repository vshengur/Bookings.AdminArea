namespace Bookings.Main.Controllers
{
    using Bookings.Bus.Queues.Messages;
    using Bookings.Domain.DTO;

    using MassTransit;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер упралвения заказами.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly ILogger<HotelsController> _logger;
        private readonly IBus _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelsController"/> class.
        /// </summary>
        /// <param name="client">GRPC client.</param>
        /// <param name="logger">Some logger.</param>
        /// <param name="bus">Bus abstraction.</param>
        public HotelsController(
            ILogger<HotelsController> logger,
            IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// Создать новую запись о бронировании.
        /// </summary>
        /// <returns>Результат оформления операции.</returns>
        [HttpPost]
        [Route("populate")]
        public async Task PopulateAsync([FromQuery]int count = 1_000)
        {
            var cities = new string[10]
            {
                "Los-Angeles, USA", "Saint-Petersburg, Russia", "Ottawa, Canada",
                "Cologne, Germany", "Herceg-Novi, Montenegro", "Marcel, France",
                "Venice, Italy", "São Paulo, Brasil", "Mombasa, Kenia", "Muharraq, Bahrain",
            };
            var abbr = new string[]
            {
                "Fashionable", "Excellent", "Unique", "Mysterious", "Unique", 
                "Graceful", "Isolated", "Secretive", "For lovers", "Memorable",
            };

            for (var i = 0; i < count; i++)
            {
                var city = cities[GetRandom().Next(0, cities.Length)];
                var bookingModel = new HotelDTO()
                {
                    City = city,
                    LocationX = GetRandom().NextDouble() * 90,
                    LocationY = GetRandom().NextDouble() * 90,
                    Name = abbr[GetRandom().Next(0, abbr.Length)] + " hotel at " + city,
                    RoomsCount = GetRandom().Next(0, 2) == 1 ? 1 : GetRandom().Next(1, 50),
                    Stars = GetRandom().Next(1, 6),
                };

                if (GetRandom().Next(0, 2) == 1)
                {
                    bookingModel.LocationX = -bookingModel.LocationX;
                }

                if (GetRandom().Next(0, 2) == 0)
                {
                    bookingModel.LocationY = -bookingModel.LocationY;
                }

                var newItem = new CreateHotelMessage()
                {
                    City = bookingModel.City,
                    Name = bookingModel.Name,
                    Stars = bookingModel.Stars,
                    RoomsCount = bookingModel.RoomsCount,
                    LocationX = bookingModel.LocationX,
                    LocationY = bookingModel.LocationY,
                };

                await _bus.Publish(newItem).ConfigureAwait(false);
            }

            Random GetRandom()
            {
                return new Random((int)DateTime.Now.Ticks);
            }
        }

        /// <summary>
        /// Создать новую запись о бронировании.
        /// </summary>
        /// <param name="bookingModel">Модель нового бронирования.</param>
        /// <returns>Результат оформления операции.</returns>
        [HttpPost]
        public async Task PostAsync([FromBody] HotelDTO bookingModel)
        {
            var newItem = new CreateHotelMessage()
            {
                City = bookingModel.City,
                Name = bookingModel.Name,
                Stars = bookingModel.Stars,
                RoomsCount = bookingModel.RoomsCount,
                LocationX = bookingModel.LocationX,
                LocationY = bookingModel.LocationY,
            };

            await _bus.Publish(newItem).ConfigureAwait(false);
        }

        /// <summary>
        /// Обновить запись о бронировании.
        /// </summary>
        /// <param name="id">Идентификатор бронирования.</param>
        /// <param name="bookingModel">Модель нового бронирования.</param>
        /// <returns>Результат оформления операции.</returns>
        [HttpPut]
        [Route("api/[controller]/{id}")]
        public async Task PutAsync(string id, [FromBody] HotelDTO bookingModel)
        {
            var newItem = new UpdateHotelMessage()
            {
                Name = bookingModel.Name,
                Stars = bookingModel.Stars,
                RoomsCount = bookingModel.RoomsCount,
            };

            await _bus.Send(newItem).ConfigureAwait(false);
        }
    }
}
