using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Domain.Queues.Messages
{
    public record UpdateBookingMessage
    {
        public string BookingId { get; init; }

        public string? BookName { get; init; }

        public double? Price { get; init; }

        public string? Category { get; init; }
    }
}
