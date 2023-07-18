using Bookings.Repositories.Contexts;
using Bookings.Repositories.Models.Settings;
using Bookings.Storage.Queues.Consumers;
using Bookings.Storage.Services;

using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS,
// visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.Configure<BookingsStoreDatabaseSettings>(options =>
{
    options.ConnectionString = builder.Configuration
        .GetSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.ConnectionString)}").Value;
    options.DatabaseName = builder.Configuration
        .GetSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.DatabaseName)}").Value;
    options.BookingsCollectionName = builder.Configuration
        .GetSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.BookingsCollectionName)}").Value;
    options.HotelsCollectionName = builder.Configuration
        .GetSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.HotelsCollectionName)}").Value;
    options.ClientsCollectionName = builder.Configuration
        .GetSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.ClientsCollectionName)}").Value;
});
builder.Services.AddSingleton<IMongoDBContext, MongoBookingsDBContext>();

builder.Configuration.AddEnvironmentVariables(prefix: "Bookings_Storage_");

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<CreateBookingConsumer>();
    x.AddConsumer<CreateHotelConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration.GetSection($"RabbitMqHost").Value;

        cfg.Host(rabbitMqHost, "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BookingsService>();
app.MapGet("/", 
    () => "Communication with gRPC endpoints must be made through a gRPC client. " +
    "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
