using Bookings.Consumers.Messages;

using MassTransit;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Consumers.Consumers
{
    public class CreateBookingConsumer :
        IConsumer<CreateBooking>
    {
        readonly ILogger<CreateBookingConsumer> _logger;

        public CreateBookingConsumer(ILogger<CreateBookingConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CreateBooking> context)
        {
            _logger.LogInformation($"Creating ");
            return Task.CompletedTask;
        }
    }
}
