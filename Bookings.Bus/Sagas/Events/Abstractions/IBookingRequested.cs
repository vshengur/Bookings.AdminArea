using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Bus.Sagas.Events.Abstractions
{
    public interface IBookingRequested : IBookingBaseRequest
    {
        // Guid BookingId { get; set; }

        DateTime Timestamp { get; set; }

        // Дополнительные свойства для запроса бронирования
        string BookName { get; set; }

        double Price { get; set; }

        string Category { get; set; }

        string HotelId { get; set; }
    }

    public interface IBookingBaseRequest
    {
        public Guid CorrelationId { get; set; }
    }
}
