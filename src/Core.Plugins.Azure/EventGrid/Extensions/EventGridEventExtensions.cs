using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using System;

namespace Core.Plugins.Azure.EventGrid.Extensions
{
    public static class EventGridEventExtensions
    {
        public static TPayload GetPayload<TPayload>(this EventGridEvent eventGridEvent)
        {
            if (eventGridEvent.Data == null || string.IsNullOrEmpty(eventGridEvent.Data.ToString()))
            {
                throw new InvalidOperationException("EventGridEvent.Data property cannot be null or empty");
            }

            return JsonConvert.DeserializeObject<TPayload>(eventGridEvent.Data.ToString());
        }
    }
}
