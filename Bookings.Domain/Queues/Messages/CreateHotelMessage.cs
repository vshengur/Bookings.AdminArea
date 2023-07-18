using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Domain.Queues.Messages
{
    public record CreateHotelMessage
    {
        public string Name { get; set; }

        public string City { get; set; }

        public int Stars { get; set; }

        public int RoomsCount { get; set; }

        public double LocationX { get; set; }

        public double LocationY { get; set; }
    }
}
