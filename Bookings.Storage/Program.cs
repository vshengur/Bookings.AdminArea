using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain;
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

builder.Configuration.AddEnvironmentVariables(prefix: "Bookings_Storage_");

builder.Services.Configure<BookingsStoreDatabaseSettings>(options =>
{
    var user = builder.Configuration
        .GetRequiredSection("DB_USER").Value;
    var password = builder.Configuration
        .GetRequiredSection("DB_PASSWORD").Value;
    var host = builder.Configuration
        .GetRequiredSection("DB_HOST").Value;
    var port = builder.Configuration
        .GetRequiredSection("DB_PORT").Value;

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
builder.Services.AddSingleton<IMongoDBContext, MongoBookingsDBContext>();
builder.Services.AddTransient<BookingsRepository>();
builder.Services.AddTransient<HotelsRepository>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<CreateBookingConsumer>();
    x.AddConsumer<CreateHotelConsumer>();

    x.AddConsumer<BookingRequestedConsumer>();
    x.AddRequestClient<BookingRequestedConsumer>();

    x.AddConsumer<BookingConfirmedConsumer>();
    x.AddRequestClient<BookingConfirmedConsumer>();

    x.AddConsumer<BookingCancelledConsumer>();
    x.AddRequestClient<BookingCancelledConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration.GetRequiredSection($"RabbitMq_Host").Value;

        cfg.Host(rabbitMqHost, "/", h => {
            h.Username(builder.Configuration.GetRequiredSection($"RabbitMq_User").Value);
            h.Password(builder.Configuration.GetRequiredSection($"RabbitMq_Password").Value);
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
