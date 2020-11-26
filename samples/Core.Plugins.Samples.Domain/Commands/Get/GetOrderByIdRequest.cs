using MediatR;

namespace Core.Plugins.Samples.Domain.Commands.Get
{
    public class GetOrderByIdRequest : IRequest<GetOrderByIdResponse>
    {
        public long OrderId { get; set; }
    }
}
