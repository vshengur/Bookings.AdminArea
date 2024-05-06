// <copyright file="HotelsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Bookings.Main.Controllers;

using Bookings.Bus.Queues.Messages;
using Bookings.Domain.Dto;
using Bookings.Domain.Mappers;
using Bookings.Repositories.Domain.Interfaces;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер упралвения заказами.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HotelsController"/> class.
/// </remarks>
/// <param name="client">GRPC client.</param>
/// <param name="logger">Some logger.</param>
/// <param name="bus">Bus abstraction.</param>
[ApiController]
[Route("api/[controller]")]
public class HotelsController(ILogger<HotelsController> logger, IBus bus, IHotelsRepository repository)
    : ControllerBase
{
    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="bookingModel">Модель нового бронирования.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpGet]
    public async Task<List<HotelDto>> GetHotels()
    {
        var hotels = await repository.Get(0, 20);
        var mapper = new DtoMapper();
        return hotels.Select(_ => mapper.HotelToHotelDto(_)).ToList();
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="bookingModel">Модель нового бронирования.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPost]
    public async Task PostAsync([FromBody] HotelDto bookingModel)
    {
        var newItem = new CreateHotelMessage()
        {
            City = bookingModel.City,
            Country = bookingModel.Country,
            Name = bookingModel.Name,
            Stars = bookingModel.Stars,
            Rate = bookingModel.Rate,
            LocationX = bookingModel.LocationX,
            LocationY = bookingModel.LocationY,
        };

        await bus.Publish(newItem).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновить запись о бронировании.
    /// </summary>
    /// <param name="id">Идентификатор отеля.</param>
    /// <param name="model">Данные отеля.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPut]
    [Route("api/[controller]/{id}")]
    public async Task PutAsync(string id, [FromBody] HotelDto model)
    {
        var newItem = new UpdateHotelMessage()
        {
            HotelId = id,
            Name = model.Name,
            Stars = model.Stars,
            Rate = model.Rate,
        };

        await bus.Send(newItem).ConfigureAwait(false);
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="count">Количество отелей.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPost]
    [Route("populate")]
    public async Task PopulateAsync([FromQuery] int count = 100)
    {
        var cities = new (string Name, string Country)[10]
        {
            ("Los-Angeles", "USA"), ("Saint-Petersburg", "Russia"), ("Ottawa", "Canada"),
            ("Cologne", "Germany"), ("Herceg-Novi", "Montenegro"), ("Marcel", "France"),
            ("Venice", "Italy"), ("São Paulo", "Brasil"), ("Mombasa", "Kenia"), ("Muharraq", "Bahrain"),
        };
        var abbr = new string[]
        {
            "Fashionable", "Excellent", "Mysterious", "Unique",
            "Graceful", "Isolated", "Secretive", "For lovers", "Memorable",
        };

        for (var i = 0; i < count; i++)
        {
            var city = cities[GetRandom().Next(0, cities.Length)];

            var hotelModel = new CreateHotelMessage()
            {
                City = city.Name,
                Country = city.Country,
                LocationX = GetRandom().NextDouble() * 90,
                LocationY = GetRandom().NextDouble() * 90,
                Name = abbr[GetRandom().Next(0, abbr.Length)] + " hotel at " + city,
                Rate = GetRandom().Next(0, 2) == 1 ? 9 : GetRandom().Next(1, 11),
                Stars = GetRandom().Next(1, 6),
            };

            if (GetRandom().Next(0, 2) == 1)
            {
                hotelModel.LocationX = -hotelModel.LocationX;
            }

            if (GetRandom().Next(0, 2) == 0)
            {
                hotelModel.LocationY = -hotelModel.LocationY;
            }

            await bus.Publish(hotelModel).ConfigureAwait(false);
        }

        static Random GetRandom() => new ((int)DateTime.Now.Ticks);
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="id">Идентификатор отеля.</param>
    /// <param name="model">Данные номера отеля.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPost]
    [Route("api/[controller]/{id}/rooms")]
    public async Task PostRoomAsync(string id, [FromBody] RoomDto model)
    {
        var newItem = new CreateRoomMessage()
        {
            Name = model.Name,
            AdditionalFreeKids = model.AdditionalFreeKids,
            HotelId = id,
            MaxPersons = model.MaxPersons,
        };

        await bus.Publish(newItem).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновить запись о бронировании.
    /// </summary>
    /// <param name="id">Идентификатор отеля.</param>
    /// <param name="roomId">Идентификатор номера отеля.</param>
    /// <param name="model">Данные номера отеля.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPut]
    [Route("api/[controller]/{id}/rooms/{roomId}")]
    public async Task PutRoomAsync(string id, string roomId, [FromBody] RoomDto model)
    {
        var newItem = new UpdateRoomMessage()
        {
            HotelId = id,
            RoomId = roomId,
            Name = model.Name,
            MaxPersons = model.MaxPersons,
            AdditionalFreeKids = model.AdditionalFreeKids,
        };

        await bus.Send(newItem).ConfigureAwait(false);
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="id">Идентификатор отеля.</param>
    /// <param name="maxCount">Максимальное количество номеров в отелей.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPost]
    [Route("api/[controller]/{id}/rooms/populate")]
    public async Task PopulateRoomsAsync(string id, [FromQuery] int maxCount = 5)
    {
        var abbr = new string[]
        {
            "Fashionable", "Excellent", "Mysterious", "Unique",
            "Graceful", "Isolated", "Secretive", "For lovers", "Memorable",
        };

        for (var i = 0; i < GetRandom().Next(1, maxCount + 1); i++)
        {
            var hotelModel = new CreateRoomMessage()
            {
                AdditionalFreeKids = GetRandom().Next(0, 3),
                MaxPersons = GetRandom().Next(0, 5),
                HotelId = id,
                Name = abbr[GetRandom().Next(0, abbr.Length)] + " room",
            };

            await bus.Publish(hotelModel).ConfigureAwait(false);
        }

        static Random GetRandom() => new((int)DateTime.Now.Ticks);
    }
}
