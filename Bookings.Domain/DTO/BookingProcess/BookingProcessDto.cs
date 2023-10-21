using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Domain.DTO.BookingProcess
{
    public class BookingProcessDto
    {
        public string BookingId { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
