using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain.Interfaces;
using Bookings.Repositories.Domain;

using Microsoft.Extensions.DependencyInjection;

using ServiceCollection.Extensions.Modules;
using Bookings.Infrastructure.Services.Abstractions;
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
            services.AddSingleton<IMongoDBContext, MongoBookingsDBContext>();

            services.AddTransient<IBookingsRepository, BookingsRepository>();
            services.AddTransient<IHotelsRepository, HotelsRepository>();
            services.AddTransient<IRoomsRepository, RoomsRepository>();

            services.AddTransient<IBookingStateService, BookingStateService>();

            services.AddScoped<IDocumentMapper<Booking, BookingDocument>, BookingsMapper>();
            services.AddScoped<IDocumentMapper<Hotel, HotelDocument>, HotelsMapper>();
            services.AddScoped<IDocumentMapper<Room, RoomDocument>, RoomsMapper>();
        }
    }
}
