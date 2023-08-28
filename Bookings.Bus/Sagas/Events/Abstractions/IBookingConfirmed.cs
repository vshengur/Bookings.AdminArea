using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Bus.Sagas.Events.Abstractions
{
    public interface IBookingConfirmed: IBookingBaseRequest
    {
        string BookingId { get; }
        DateTime Timestamp { get; }
        // Дополнительные свойства для подтверждения бронирования
    }
}
