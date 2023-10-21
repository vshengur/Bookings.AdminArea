#pragma warning disable SA1200 // Using directives should be placed correctly
using Bookings.Bus.Sagas.StateMachine;
using Bookings.Bus.Sagas.States;
using Bookings.Services.Implementations;
using Bookings.Services.Interfaces;
using Bookings.Web;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables(prefix: "Api_Storage_");

builder.Services.AddMassTransit(x =>
{
    var user = builder.Configuration.GetRequiredSection("DB_USER").Value;
    var password = builder.Configuration.GetRequiredSection("DB_PASSWORD").Value;
    var host = builder.Configuration.GetRequiredSection("DB_HOST").Value;
    var port = builder.Configuration.GetRequiredSection("DB_PORT").Value;

    var databaseName = builder.Configuration.GetSection($"BookingDatabase:DatabaseName").Value;
    var bookingsStateCollectionName = builder.Configuration.GetSection($"BookingDatabase:BookingsStateCollectionName").Value;

    // Настройка MongoDb
    var mongoUrl = $"mongodb://{user}:{password}@{host}:{port}/?authMechanism=SCRAM-SHA-256";

    x.SetKebabCaseEndpointNameFormatter();
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

var grpcMethodConfig = new MethodConfig()
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(3),
        MaxBackoff = TimeSpan.FromSeconds(50),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable },
    },
};

builder.Services.AddGrpcClient<BookingsContract.BookingsContractClient>(o =>
{
    o.Address = new Uri("https://localhost:7073");
})
.ConfigureChannel(o =>
{
    o.Credentials = ChannelCredentials.SecureSsl;
    o.ServiceConfig = new ServiceConfig
    {
        LoadBalancingConfigs = { new RoundRobinConfig() },
        MethodConfigs = { grpcMethodConfig },
    };
});

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

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;

        await context.Response.WriteAsync("An exception was thrown.");

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
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
