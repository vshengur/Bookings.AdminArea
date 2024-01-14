using Bookings.Bus.Sagas.Events.Abstractions;
using MassTransit;

using System.Runtime.CompilerServices;

namespace Bookings.Bus.Sagas.Events.Realizations;

public record BookingConfirmed : BaseBookingRequest, IBookingConfirmed
{
    public string HotelId { get; set; }

    public string BookingId { get; set; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<BookingConfirmed>(x => x.CorrelationId);
    }
}
