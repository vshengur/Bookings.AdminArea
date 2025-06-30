using Bookings.Domain.Repositories;
using Bookings.PostgresRepositories.Contexts;
using Bookings.PostgresRepositories.Models.Settings;
using Bookings.PostgresRepositories.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using ServiceCollection.Extensions.Modules;

namespace Bookings.PostgresRepositories;

public class PostgresRepositoriesModule : Module
{
    private readonly IConfiguration _configuration;

    public PostgresRepositoriesModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(IServiceCollection services)
    {
        base.Load(services);

        // Регистрация настроек
        IConfigurationSection section = _configuration.GetSection("PostgresDatabase");
        services.Configure<PostgresDatabaseSettings>(options =>
            _configuration.GetSection("PostgresDatabase").Bind(options));

        // Регистрация DbContext
        services.AddDbContext<BookingsDbContext>((serviceProvider, options) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<PostgresDatabaseSettings>>().Value;
            options.UseNpgsql(settings.ConnectionString);
        });

        // Регистрация репозиториев
        services.AddTransient<IBookingsRepository, PostgresBookingsRepository>();
        services.AddTransient<IBookingStateRepository, PostgresBookingStateRepository>();
    }
} 