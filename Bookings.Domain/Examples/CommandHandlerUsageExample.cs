using Bookings.Domain.Commands;
using Bookings.Domain.Handlers;
using Bookings.Domain.Handlers.Implementations;
using Bookings.Domain.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Bookings.Domain.Examples;

/// <summary>
/// Примеры использования новой архитектуры с командами и валидацией
/// </summary>
public static class CommandHandlerUsageExample
{
    /// <summary>
    /// Пример настройки DI контейнера
    /// </summary>
    public static void ConfigureServices(IServiceCollection services)
    {
        // Регистрация валидаторов
        services.AddScoped<IValidator<CreateHotelCommand>, CreateHotelCommandValidator>();
        services.AddScoped<IValidator<CreateRoomCommand>, CreateRoomCommandValidator>();
        services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingCommandValidator>();

        // Регистрация обработчиков
        services.AddScoped<ICommandHandler<CreateHotelCommand, Hotel>, CreateHotelCommandHandler>();
        services.AddScoped<ICommandHandler<CreateRoomCommand, Room>, CreateRoomCommandHandler>();
        services.AddScoped<ICommandHandler<CreateBookingCommand, Booking>, CreateBookingCommandHandler>();

        // Регистрация декораторов
        services.Decorate<ICommandHandler<CreateHotelCommand, Hotel>, ValidationCommandHandlerDecorator<CreateHotelCommand, Hotel>>();
        services.Decorate<ICommandHandler<CreateRoomCommand, Room>, ValidationCommandHandlerDecorator<CreateRoomCommand, Room>>();
        services.Decorate<ICommandHandler<CreateBookingCommand, Booking>, ValidationCommandHandlerDecorator<CreateBookingCommand, Booking>>();
    }

    /// <summary>
    /// Пример создания отеля с валидацией
    /// </summary>
    public static async Task<Hotel> CreateHotelExample(ICommandHandler<CreateHotelCommand, Hotel> handler)
    {
        var command = new CreateHotelCommand
        {
            Name = "Grand Hotel",
            City = "Москва",
            Country = "Россия",
            Stars = 5,
            Rate = 9,
            LocationX = 55.7558,
            LocationY = 37.6176
        };

        try
        {
            // Валидация происходит автоматически в декораторе
            var hotel = await handler.HandleAsync(command);
            Console.WriteLine($"Отель создан: {hotel.Name} в {hotel.City}");
            return hotel;
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("Ошибки валидации:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
            throw;
        }
    }

    /// <summary>
    /// Пример создания номера с валидацией
    /// </summary>
    public static async Task<Room> CreateRoomExample(ICommandHandler<CreateRoomCommand, Room> handler, string hotelId)
    {
        var command = new CreateRoomCommand
        {
            Name = "Люкс",
            MaxPersons = 4,
            AdditionalFreeKids = 2,
            HotelId = hotelId
        };

        try
        {
            var room = await handler.HandleAsync(command);
            Console.WriteLine($"Номер создан: {room.Name} для {room.MaxPersons} человек");
            return room;
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("Ошибки валидации:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
            throw;
        }
    }

    /// <summary>
    /// Пример создания бронирования с валидацией
    /// </summary>
    public static async Task<Booking> CreateBookingExample(ICommandHandler<CreateBookingCommand, Booking> handler, string roomId)
    {
        var command = new CreateBookingCommand
        {
            BookName = "Бронирование Иванова",
            RoomId = roomId,
            Price = 5000.0,
            Category = "Стандарт",
            StateId = Guid.NewGuid(),
            StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
            Adults = 2,
            Kids = 1
        };

        try
        {
            var booking = await handler.HandleAsync(command);
            Console.WriteLine($"Бронирование создано: {booking.BookName} на {booking.GetTotalDays()} дней");
            return booking;
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("Ошибки валидации:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
            throw;
        }
    }

    /// <summary>
    /// Пример обработки ошибок валидации
    /// </summary>
    public static async Task HandleValidationErrorsExample(ICommandHandler<CreateHotelCommand, Hotel> handler)
    {
        // Попытка создать отель с невалидными данными
        var invalidCommand = new CreateHotelCommand
        {
            Name = "", // Пустое имя
            City = "Москва",
            Country = "Россия",
            Stars = 6, // Больше 5 звезд
            Rate = 11, // Больше 10
            LocationX = 200, // Вне диапазона
            LocationY = 37.6176
        };

        try
        {
            await handler.HandleAsync(invalidCommand);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("Найдены ошибки валидации:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.PropertyName}: {error.ErrorMessage}");
            }
        }
    }

    /// <summary>
    /// Пример использования в контроллере
    /// </summary>
    public static async Task<object> ControllerExample(ICommandHandler<CreateHotelCommand, Hotel> handler, CreateHotelCommand command)
    {
        try
        {
            var hotel = await handler.HandleAsync(command);
            
            // Возвращаем успешный результат
            return new
            {
                Success = true,
                Data = hotel,
                Message = "Отель успешно создан"
            };
        }
        catch (ValidationException ex)
        {
            // Возвращаем ошибки валидации
            return new
            {
                Success = false,
                Errors = ex.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Message = e.ErrorMessage
                }),
                Message = "Ошибки валидации"
            };
        }
        catch (Exception ex)
        {
            // Обработка других ошибок
            return new
            {
                Success = false,
                Message = "Произошла ошибка при создании отеля",
                Error = ex.Message
            };
        }
    }
} 