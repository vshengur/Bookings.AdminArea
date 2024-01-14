// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly;
using Bookings.Api.Consumers;
using Bookings.Api.Endpoints;
using Bookings.Bus.Sagas.StateMachine;
using Bookings.Bus.Sagas.States;
using Bookings.Domain;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;
using Bookings.Infrastructure.Services.Abstractions;
using Bookings.Infrastructure.Services.Implementations;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain;
using Bookings.Repositories.Domain.Interfaces;
using Bookings.Repositories.Models.Settings;

using Grpc.Core;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics;

using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables(prefix: "Api_");

var user = builder.Configuration.GetRequiredSection("DB_USER").Value;
var password = builder.Configuration.GetRequiredSection("DB_PASSWORD").Value;
var host = builder.Configuration.GetRequiredSection("DB_HOST").Value;
var port = builder.Configuration.GetRequiredSection("DB_PORT").Value;

builder.Services.Configure<BookingsStoreDatabaseSettings>(options =>
{
    options.ConnectionString = $"mongodb://{user}:{password}@{host}:{port}/?authMechanism=SCRAM-SHA-256";

    options.DatabaseName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.DatabaseName)}")!.Value!;
    options.BookingsCollectionName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.BookingsCollectionName)}").Value!;
    options.HotelsCollectionName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.HotelsCollectionName)}").Value!;
    options.ClientsCollectionName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.ClientsCollectionName)}").Value!;
});

builder.Services.AddMassTransit(x =>
{
    var databaseName = builder.Configuration.GetSection($"BookingDatabase:DatabaseName").Value;
    var bookingsStateCollectionName = builder.Configuration.GetSection($"BookingDatabase:BookingsStateCollectionName").Value;

    // Настройка MongoDb
    var mongoUrl = $"mongodb://{user}:{password}@{host}:{port}/?authMechanism=SCRAM-SHA-256";

    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<CreateHotelConsumer>();
    x.AddConsumer<CreateRoomConsumer>();

    // Sagas consumers
    x.AddConsumer<BookingRequestedConsumer>();
    x.AddRequestClient<BookingRequestedConsumer>();

    x.AddConsumer<BookingConfirmedConsumer>();
    x.AddRequestClient<BookingConfirmedConsumer>();

    x.AddConsumer<BookingCancelledConsumer>();
    x.AddRequestClient<BookingCancelledConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetRequiredSection($"RabbitMq_Host").Value, "/", h =>
        {
            h.Username(builder.Configuration.GetRequiredSection($"RabbitMq_User").Value);
            h.Password(builder.Configuration.GetRequiredSection($"RabbitMq_Password").Value);
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddSagaStateMachine<BookingMainSaga, BookingMainSagaState>()
        .MongoDbRepository(r =>
        {
            r.Connection = mongoUrl;
            r.DatabaseName = databaseName;
            r.CollectionName = bookingsStateCollectionName;
        });
});

builder.Services.AddSingleton<IMongoDBContext, MongoBookingsDBContext>();

builder.Services.AddScoped<IDocumentMapper<Booking, BookingDocument>, BookingsMapper>();
builder.Services.AddScoped<IDocumentMapper<Hotel, HotelDocument>, HotelsMapper>();
builder.Services.AddScoped<IDocumentMapper<Room, RoomDocument>, RoomsMapper>();

builder.Services.AddTransient<IBookingsRepository, BookingsRepository>();
builder.Services.AddTransient<IHotelsRepository, HotelsRepository>();
builder.Services.AddTransient<IRoomsRepository, RoomsRepository>();

builder.Services.AddTransient<IBookingStateService, BookingStateService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = Text.Plain;

        await context.Response.WriteAsync("An exception was thrown.");

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
        }

        if (exceptionHandlerPathFeature?.Error is RpcException
            && ((RpcException)exceptionHandlerPathFeature.Error).StatusCode == StatusCode.DeadlineExceeded)
        {
            await context.Response.WriteAsync(" The rpc ex." + exceptionHandlerPathFeature?.Error.Message);
        }

        if (exceptionHandlerPathFeature?.Error is RpcException)
        {
            await context.Response.WriteAsync(" The rpc ex." + exceptionHandlerPathFeature?.Error.Message);
        }

        if (exceptionHandlerPathFeature?.Path == "/")
        {
            await context.Response.WriteAsync(" Page: Home.");
        }
    });
});

// Configure the HTTP request pipeline.
app.MapGrpcService<BookingsEndpoint>();
app.UseHttpsRedirection();

app.MapControllers();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // Add Swagger Api
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.Run();
