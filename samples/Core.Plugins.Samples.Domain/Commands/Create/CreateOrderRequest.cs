using Core.Plugins.Samples.Domain.DTO;
using MediatR;

namespace Core.Plugins.Samples.Domain.Commands.Create
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public Order Order { get; set; }
    }
}
