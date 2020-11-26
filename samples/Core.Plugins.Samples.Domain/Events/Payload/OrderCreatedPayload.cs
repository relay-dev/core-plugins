using System;

namespace Core.Plugins.Samples.Domain.Events.Payload
{
    public class OrderCreatedPayload
    {
        public long OrderId { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
