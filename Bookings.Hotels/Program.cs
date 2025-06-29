using Bookings.Repositories.Models.Settings;
using Bookings.Repositories;
using MassTransit;
using ServiceCollection.Extensions.Modules;
using Bookings.Hotels.Consumers;
using Bookings.Repositories.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables(prefix: "Hotels_");

var user = builder.Configuration.GetRequiredSection("DB_USER").Value;
var password = builder.Configuration.GetRequiredSection("DB_PASSWORD").Value;
var host = builder.Configuration.GetRequiredSection("DB_HOST").Value;
var port = builder.Configuration.GetRequiredSection("DB_PORT").Value;

builder.Services.Configure<BookingsStoreDatabaseSettings>(options =>
{
    options.ConnectionString = $"mongodb://{user}:{password}@{host}:{port}/?authMechanism=SCRAM-SHA-256";

    options.DatabaseName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.DatabaseName)}")!.Value!;
    options.HotelsCollectionName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.HotelsCollectionName)}").Value!;
    options.RoomsCollectionName = builder.Configuration
        .GetRequiredSection($"BookingDatabase:{nameof(BookingsStoreDatabaseSettings.RoomsCollectionName)}").Value!;
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // Basic consumers
    x.AddConsumer<CreateHotelConsumer>();
    x.AddConsumer<CreateRoomConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetRequiredSection($"RabbitMq_Host").Value, "/", h =>
        {
            h.Username(builder.Configuration.GetRequiredSection($"RabbitMq_User").Value);
            h.Password(builder.Configuration.GetRequiredSection($"RabbitMq_Password").Value);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.RegisterModule<RepositoriesModule>();
builder.Services.AddSingleton<IMongoDBContext, MongoHotelsDBContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
