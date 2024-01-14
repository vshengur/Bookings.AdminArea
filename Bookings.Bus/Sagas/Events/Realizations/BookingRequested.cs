using Bookings.Bus.Sagas.Events.Abstractions;

using MassTransit;

using System.Runtime.CompilerServices;

namespace Bookings.Bus.Sagas.Events.Realizations;

public record BookingRequested : BaseBookingRequest, IBookingRequested
{
    public Guid StateId { get; set; }

    public string BookName { get; set; }

    public double Price { get; set; }

    public string Category { get; set; }

    public string RoomId { get; set; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<BookingRequested>(x => x.CorrelationId);
    }
}
