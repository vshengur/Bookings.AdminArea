using Bookings.Domain.Repositories;
using Bookings.Repositories.Domain;

using Microsoft.Extensions.DependencyInjection;

using ServiceCollection.Extensions.Modules;
using Bookings.Domain.Services;
using Bookings.Infrastructure.Services.Implementations;
using Bookings.Domain;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;

namespace Bookings.Repositories
{
    public class RepositoriesModule : Module
    {
        protected override void Load(IServiceCollection services)
        {
            base.Load(services);

            // MongoDB репозитории только для Hotels и Rooms
            services.AddTransient<IHotelsRepository, HotelsRepository>();
            services.AddTransient<IRoomsRepository, RoomsRepository>();

            // Сервисы остаются в MongoDB
            services.AddTransient<BookingRepositoryAdapter>();
            services.AddTransient<IBookingStateService, BookingStateService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IBookingQueryService, BookingQueryService>();

            // Мапперы для MongoDB документов
            services.AddScoped<IDocumentMapper<Hotel, HotelDocument>, HotelsMapper>();
            services.AddScoped<IDocumentMapper<Room, RoomDocument>, RoomsMapper>();
        }
    }
}
