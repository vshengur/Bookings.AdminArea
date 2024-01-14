using Bookings.Bus.Sagas.Events.Abstractions;
using MassTransit;

using System.Runtime.CompilerServices;

namespace Bookings.Bus.Sagas.Events.Realizations;

public record BookingCancelled : BaseBookingRequest, IBookingCancelled
{
    public string CancelReason { get; set; }

    public string HotelId { get; set; }

    public string BookingId { get; set; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<BookingRequested>(x => x.CorrelationId);
    }
}
