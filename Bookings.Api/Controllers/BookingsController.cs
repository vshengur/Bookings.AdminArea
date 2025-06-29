// <copyright file="BookingsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Bookings.Api.Controllers;

using Bookings.Bus.Queues.Messages;
using Bookings.Domain.Dto;
using Bookings.Domain.Services;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер управления бронированиями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly ILogger<BookingsController> logger;
    private readonly IBookingService bookingService;
    private readonly IBookingQueryService bookingQueryService;
    private readonly IBookingStateService bookingStateService;
    private readonly IBus bus;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookingsController"/> class.
    /// </summary>
    /// <param name="logger">Some logger.</param>
    /// <param name="bookingService">BookingService abstraction.</param>
    /// <param name="bookingQueryService">BookingQueryService abstraction.</param>
    /// <param name="bookingStateService">BookingStateService abstraction.</param>
    /// <param name="bus">Bus abstraction.</param>
    public BookingsController(
        ILogger<BookingsController> logger,
        IBookingService bookingService,
        IBookingQueryService bookingQueryService,
        IBookingStateService bookingStateService,
        IBus bus)
    {
        this.logger = logger;
        this.bookingService = bookingService;
        this.bookingQueryService = bookingQueryService;
        this.bookingStateService = bookingStateService;
        this.bus = bus;
    }

    /// <summary>
    /// Получить список бронирований.
    /// </summary>
    /// <param name="page">Номер страницы</param>
    /// <param name="count">Количество элементов на странице</param>
    /// <returns>Список бронирований</returns>
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int page = 0, [FromQuery] int count = 30)
    {
        try
        {
            var bookings = await bookingQueryService.GetBookingsAsync(page, count);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении списка бронирований");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Получить бронирование по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <returns>Бронирование</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var booking = await bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении бронирования с ID {Id}", id);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Создать новое бронирование.
    /// </summary>
    /// <param name="bookingDto">Данные бронирования</param>
    /// <returns>Созданное бронирование</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] BookingDto bookingDto)
    {
        try
        {
            var createdBooking = await bookingService.CreateBookingAsync(bookingDto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdBooking.BookingId }, createdBooking);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании бронирования");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Обновить бронирование.
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <param name="bookingDto">Новые данные бронирования</param>
    /// <returns>Обновленное бронирование</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] BookingDto bookingDto)
    {
        try
        {
            var updatedBooking = await bookingService.UpdateBookingAsync(id, bookingDto);
            return Ok(updatedBooking);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении бронирования с ID {Id}", id);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Удалить бронирование.
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <returns>Результат операции</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            var result = await bookingService.DeleteBookingAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при удалении бронирования с ID {Id}", id);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    /// <summary>
    /// Создать новую запись о бронировании через состояние.
    /// </summary>
    /// <param name="bookingModel">Модель нового бронирования.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPost("state")]
    public async Task<IActionResult> PostStateAsync([FromBody] BookingDto bookingModel)
    {
        var result = await bookingStateService.ProcessRequest(bookingModel);

        return result == null ? new NoContentResult() : new JsonResult(result);
    }

    /// <summary>
    /// Отменить бронирование.
    /// </summary>
    /// <param name="id">Идентификатор бронирования.</param>
    /// <returns>Результат операции.</returns>
    [HttpPut("{id}/cancel")]
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
    /// Подтвердить бронирование.
    /// </summary>
    /// <param name="id">Идентификатор бронирования.</param>
    /// <returns>Результат операции.</returns>
    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> ConfirmBooking(string id)
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
    /// Обновить запись о бронировании через сообщение.
    /// </summary>
    /// <param name="id">Идентификатор бронирования.</param>
    /// <param name="bookingModel">Модель нового бронирования.</param>
    /// <returns>Результат оформления операции.</returns>
    [HttpPut("{id}/message")]
    public async Task<IActionResult> UpdateViaMessageAsync(string id, [FromBody] BookingDto bookingModel)
    {
        var newItem = new UpdateBookingMessage()
        {
            BookingId = id,
            BookName = bookingModel.BookName,
            Category = bookingModel.Category,
            Price = bookingModel.Price,
        };

        await bus.Send(newItem).ConfigureAwait(false);
        return Accepted();
    }
}