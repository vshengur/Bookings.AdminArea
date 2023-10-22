// <copyright file="BookingsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Bookings.Web.Controllers;

using Bookings.Bus.Queues.Messages;
using Bookings.Domain.DTO;
using Bookings.Services.Interfaces;
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
    private readonly BookingsContract.BookingsContractClient grpcBookingClient;
    private readonly ILogger<BookingsController> logger;
    private readonly IBookingStateService bookingStateService;
    private readonly IBus bus;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookingsController"/> class.
    /// </summary>
    /// <param name="grpcBookingClient">gRPC booking client.</param>
    /// <param name="logger">Some logger.</param>
    /// <param name="bookingStateService">BookingStateService abstraction.</param>
    public BookingsController(
        BookingsContract.BookingsContractClient grpcBookingClient,
        ILogger<BookingsController> logger,
        IBookingStateService bookingStateService,
        IBus bus)
    {
        this.grpcBookingClient = grpcBookingClient;
        this.logger = logger;
        this.bookingStateService = bookingStateService;
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
            var bookings = await this.grpcBookingClient.GetBookingsAsync(
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
        var result = await bookingStateService.ProcessRequest(bookingModel);

        return result == null ? new NoContentResult() : new JsonResult(result);
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="bookingModel">Модель нового бронирования.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] BookingDTO bookingModel)
    {
        var result = await bookingStateService.ProcessRequest(bookingModel);

        return result == null ? new NoContentResult() : new JsonResult(result);
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

        await bus.Send(newItem).ConfigureAwait(false);
    }
}