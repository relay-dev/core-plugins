using Core.Events;
using Core.Plugins.Application;
using Core.Providers;
using Core.Utilities;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
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
        private readonly IConnectionStringProvider _connectionStringProvider;

        public EventGridEventClient(
            IJsonSerializer jsonSerializer,
            IDateTimeProvider dateTimeProvider,
            IEventGridClient eventGridClient,
            IConnectionStringProvider connectionStringProvider)
        {
            _jsonSerializer = jsonSerializer;
            _dateTimeProvider = dateTimeProvider;
            _eventGridClient = eventGridClient;
            _connectionStringProvider = connectionStringProvider;
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

            await _eventGridClient.PublishEventsAsync(TopicHostname, events, cancellationToken);

            return e.Id;
        }

        private string TopicHostname => new Uri(new ConnectionStringParser().Parse(_connectionStringProvider.Get("DefaultEventGridConnection"))["Endpoint"]).Host;
    }
}
