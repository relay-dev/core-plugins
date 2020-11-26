using AutoMapper;
using Core.Plugins.Samples.Domain.Context;
using Core.Plugins.Samples.Domain.DTO;
using Core.Plugins.Samples.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Samples.Domain.Commands.Get
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly OrderContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(
            OrderContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            OrderEntity orderEntity = await _dbContext.Orders
                .Where(o => o.OrderId == request.OrderId)
                .SingleOrDefaultAsync(cancellationToken);

            var order = _mapper.Map<Order>(orderEntity);

            return new GetOrderByIdResponse
            {
                Order = order
            };
        }
    }
}
