using Core.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Samples.Domain.Events.Client
{
    public class MockEventClient : IEventClient
    {
        public async Task<string> RaiseEventAsync(string subscriptionEventType, object data, CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<string> RaiseEventAsync(string subscriptionEventType, string subject, object data, CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<string> RaiseEventAsync(Event e, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
