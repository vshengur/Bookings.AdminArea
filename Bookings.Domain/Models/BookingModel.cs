using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Domain.Models
{
    public class BookingModel
    {
        public double Price { get; set; }

        public string BookName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Category { get; set; }

        public string HotelId { get; set; }
    }
}
