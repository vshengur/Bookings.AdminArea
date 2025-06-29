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

            services.AddTransient<IBookingsRepository, BookingsRepository>();
            services.AddTransient<IHotelsRepository, HotelsRepository>();
            services.AddTransient<IRoomsRepository, RoomsRepository>();

            services.AddTransient<BookingRepositoryAdapter>();
            services.AddTransient<IBookingStateService, BookingStateService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IBookingQueryService, BookingQueryService>();

            services.AddScoped<IDocumentMapper<Booking, BookingDocument>, BookingsMapper>();
            services.AddScoped<IDocumentMapper<Hotel, HotelDocument>, HotelsMapper>();
            services.AddScoped<IDocumentMapper<Room, RoomDocument>, RoomsMapper>();
        }
    }
}
