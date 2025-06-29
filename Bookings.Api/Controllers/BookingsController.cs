﻿// <copyright file="BookingsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Bookings.Api.Controllers;

using Bookings.Bus.Queues.Messages;
using Bookings.Domain.Dto;
using Bookings.Infrastructure.Services.Abstractions;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер упралвения заказами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly ILogger<BookingsController> logger;
    // private readonly IBookingService bookingService;
    private readonly IBookingStateService bookingStateService;
    private readonly IBus bus;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookingsController"/> class.
    /// </summary>
    /// <param name="logger">Some logger.</param>
    /// <param name="bookingStateService">BookingStateService abstraction.</param>
    /// <param name="bookingService">BookingService abstraction.</param>
    /// <param name="bus">Bus abstraction.</param>
    public BookingsController(
        ILogger<BookingsController> logger,
        //IBookingService bookingService,
        IBookingStateService bookingStateService,
        IBus bus)
    {
        this.logger = logger;
        //this.bookingService = bookingService;
        this.bookingStateService = bookingStateService;
        this.bus = bus;
    }

    /*
    /// <summary>
    /// Получить список заказаов.
    /// </summary>
    /// <returns>Список заказов.</returns>
    [HttpGet]
    public async Task<RestApiResponse<BookingsResponse>> GetAsync([FromQuery] int page = 0, [FromQuery] int count = 30)
    {
        try
        {
            var bookings = await bookingService.GetBookingsAsync(page, count);

            return RestApiResponse<BookingsResponse>.Success(bookings);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded)
        {
            // Обработаем таймаут операции
            return RestApiResponse<BookingsResponse>.NonSuccess(ex.Message);
        }
    }

    /// <summary>
    /// Получить список заказаов.
    /// </summary>
    /// <returns>Список заказов.</returns>
    [HttpGet]
    [Route("details")]
    public async Task<RestApiResponse<BookingsResponse>> GetDeatailsAsync([FromQuery] string id)
    {
        try
        {
            var bookings = await bookingService.GetBookingsAsync(id);

            return RestApiResponse<BookingsResponse>.Success(bookings);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded)
        {
            // Обработаем таймаут операции
            return RestApiResponse<BookingsResponse>.NonSuccess(ex.Message);
        }
    }

    */

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="bookingModel">Модель нового бронирования.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] BookingDto bookingModel)
    {
        var result = await bookingStateService.ProcessRequest(bookingModel);

        return result == null ? new NoContentResult() : new JsonResult(result);
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="id">Идентификатор бронирования.</param>
    /// <returns>Результат операции.</returns>
    [HttpPut, Route("api/[controller]/{id}/cancel")]
    public async Task<IActionResult> CancelBooking(string id)
    {
        var result = await bookingStateService.ProcessRequest(
            new BookingBaseDto()
            {
                State = Domain.Dto.BookingProcess.BookingState.Cancelled,
                BookingId = id
            });

        return result == null ? new NoContentResult() : new JsonResult(result);
    }

    /// <summary>
    /// Создать новую запись о бронировании.
    /// </summary>
    /// <param name="id">Идентификатор бронирования.</param>
    /// <returns>Результат операции.</returns>
    [HttpPut, Route("api/[controller]/{id}/confirm")]
    public async Task<IActionResult> PutAsync(string id)
    {
        var result = await bookingStateService.ProcessRequest(
            new BookingBaseDto()
            {
                State = Domain.Dto.BookingProcess.BookingState.Confirmed,
                BookingId = id
            });

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
    public async Task PutAsync(string id, [FromBody] BookingDto bookingModel)
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