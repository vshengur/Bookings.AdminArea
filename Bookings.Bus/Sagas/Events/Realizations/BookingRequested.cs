using Bookings.Bus.Sagas.Events.Abstractions;

using MassTransit;

using System.Runtime.CompilerServices;

namespace Bookings.Bus.Sagas.Events.Realizations
{
    public record BookingRequested : IBookingRequested
    {
        public Guid CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }

        public string BookName { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }

        public string HotelId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<BookingRequested>(x => x.CorrelationId);
        }
    }

    public record BookingConfirmed : IBookingConfirmed
    {
        public Guid CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }

        public string BookName { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }

        public string HotelId { get; set; }

        public string BookingId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<BookingRequested>(x => x.CorrelationId);
        }
    }

    public record BookingCancelled : IBookingCancelled
    {
        public Guid CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }

        public string BookName { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }

        public string HotelId { get; set; }

        public string BookingId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<BookingRequested>(x => x.CorrelationId);
        }
    }
}
