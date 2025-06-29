using Bookings.Contracts;
using Bookings.Domain.Services;
using Bookings.Infrastructure.Services.Implementations;

using Grpc.Core;
using Grpc.Net.Client.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBookingService, BookingService>();

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
    o.Address = new Uri(builder.Configuration.GetRequiredSection($"StorageEndpoint:Address").Value);
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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();