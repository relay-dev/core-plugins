using Core.Events;
using Core.Providers;
using Core.Utilities;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Azure.EventGrid
{
    public class EventGridEventClient : IEventClient
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventGridClient _eventGridClient;

        public EventGridEventClient(
            IJsonSerializer jsonSerializer,
            IDateTimeProvider dateTimeProvider,
            IEventGridClient eventGridClient)
        {
            _jsonSerializer = jsonSerializer;
            _dateTimeProvider = dateTimeProvider;
            _eventGridClient = eventGridClient;
        }

        public async Task<string> RaiseEventAsync(Event e, CancellationToken cancellationToken)
        {
            string data = await _jsonSerializer.SerializeAsync(e.Data, cancellationToken);

            var events = new List<EventGridEvent>
            {
                new EventGridEvent
                {
                    Id = e.Id,
                    EventTime = _dateTimeProvider.Get(),
                    Topic = e.Topic,
                    EventType = e.EventType,
                    Subject = e.Subject,
                    DataVersion = e.DataVersion,
                    Data = data
                }
            };

            await _eventGridClient.PublishEventsAsync(e.Topic, events, cancellationToken);

            return e.Id;
        }
    }
}
