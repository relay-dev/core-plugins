using AutoMapper;
using Core.Events;
using Core.Plugins.Samples.Domain.Context;
using Core.Plugins.Samples.Domain.DTO;
using Core.Plugins.Samples.Domain.Entities;
using Core.Plugins.Samples.Domain.Events.Payload;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Samples.Domain.Commands.Create
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly OrderContext _dbContext;
        private readonly IValidator<Order> _validator;
        private readonly IMapper _mapper;
        private readonly IEventClient _eventClient;

        public CreateOrderHandler(
            OrderContext dbContext,
            IValidator<Order> validator,
            IMapper mapper,
            IEventClient eventClient)
        {
            _dbContext = dbContext;
            _validator = validator;
            _mapper = mapper;
            _eventClient = eventClient;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.Order, cancellationToken);

            OrderEntity orderEntity = _mapper.Map<OrderEntity>(request.Order);

            await _dbContext.AddAsync(orderEntity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await RaiseOrderCreatedEvent(orderEntity, cancellationToken);

            return new CreateOrderResponse
            {
                OrderId = orderEntity.OrderId
            };
        }
        
        private async Task RaiseOrderCreatedEvent(OrderEntity orderEntity, CancellationToken cancellationToken)
        {
            var e = new Event
            {
                EventType = "OrderCreated",
                Subject = "OrderCreated",
                Data = new OrderCreatedPayload
                {
                    OrderId = orderEntity.OrderId,
                    OrderDate = orderEntity.OrderDate
                }
            };

            await _eventClient.RaiseEventAsync(e, cancellationToken);
        }
    }
}
