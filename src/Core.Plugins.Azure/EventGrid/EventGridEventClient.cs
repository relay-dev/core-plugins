using Core.Events;
using Core.Plugins.Application;
using Core.Providers;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Azure.EventGrid
{
    public class EventGridEventClient : IEventClient
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventGridClient _eventGridClient;
        private readonly IConnectionStringProvider _connectionStringProvider;

        public EventGridEventClient(
            IDateTimeProvider dateTimeProvider,
            IEventGridClient eventGridClient,
            IConnectionStringProvider connectionStringProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _eventGridClient = eventGridClient;
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<string> RaiseEventAsync(string subscriptionEventType, object data, CancellationToken cancellationToken)
        {
            var e = new Event
            {
                EventType = subscriptionEventType,
                Data = data
            };

            return await RaiseEventAsync(e, cancellationToken);
        }

        public async Task<string> RaiseEventAsync(string subscriptionEventType, string subject, object data, CancellationToken cancellationToken)
        {
            var e = new Event
            {
                EventType = subscriptionEventType,
                Subject = subject,
                Data = data
            };

            return await RaiseEventAsync(e, cancellationToken);
        }

        public async Task<string> RaiseEventAsync(Event e, CancellationToken cancellationToken)
        {
            var events = new List<EventGridEvent>
            {
                new EventGridEvent
                {
                    Id = e.Id,
                    EventTime = _dateTimeProvider.Get(),
                    Topic = TopicName,
                    EventType = e.EventType,
                    Subject = e.Subject,
                    DataVersion = e.DataVersion,
                    Data = e.Data
                }
            };

            await _eventGridClient.PublishEventsAsync(TopicHostname, events, cancellationToken);

            return e.Id;
        }

        private string TopicHostname => new Uri(new ConnectionStringParser().Parse(_connectionStringProvider.Get("DefaultEventGridConnection"))["Endpoint"]).Host;
        private string TopicName => new ConnectionStringParser().Parse(_connectionStringProvider.Get("DefaultEventGridConnection"))["Topic"];
    }
}
