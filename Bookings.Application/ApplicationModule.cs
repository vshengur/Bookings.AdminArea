using Bookings.Application.Commands;
using Bookings.Application.Handlers;
using Bookings.Application.Handlers.Implementations;
using Bookings.Application.Validators;
using Bookings.Domain;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Application;

/// <summary>
/// Модуль для регистрации сервисов Application слоя
/// </summary>
public static class ApplicationModule
{
    /// <summary>
    /// Регистрирует все сервисы Application слоя
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с зарегистрированными Application сервисами</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Регистрация валидаторов
        services.AddScoped<IValidator<CreateHotelCommand>, CreateHotelCommandValidator>();
        services.AddScoped<IValidator<CreateRoomCommand>, CreateRoomCommandValidator>();
        services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingCommandValidator>();

        // Регистрация обработчиков команд
        services.AddScoped<ICommandHandler<CreateHotelCommand, Hotel>, CreateHotelCommandHandler>();
        services.AddScoped<ICommandHandler<CreateRoomCommand, Room>, CreateRoomCommandHandler>();
        services.AddScoped<ICommandHandler<CreateBookingCommand, Booking>, CreateBookingCommandHandler>();

        // Регистрация декораторов для валидации
        services.Decorate<ICommandHandler<CreateHotelCommand, Hotel>, ValidationCommandHandlerDecorator<CreateHotelCommand, Hotel>>();
        services.Decorate<ICommandHandler<CreateRoomCommand, Room>, ValidationCommandHandlerDecorator<CreateRoomCommand, Room>>();
        services.Decorate<ICommandHandler<CreateBookingCommand, Booking>, ValidationCommandHandlerDecorator<CreateBookingCommand, Booking>>();

        return services;
    }
} 