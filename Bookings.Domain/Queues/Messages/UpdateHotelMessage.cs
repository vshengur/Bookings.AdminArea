using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Domain.Queues.Messages
{
    public class UpdateHotelMessage
    {
        public string Name { get; set; }

        public int Stars { get; set; }

        public int RoomsCount { get; set; }
    }
}
