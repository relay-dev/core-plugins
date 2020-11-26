using FluentValidation;
using System;
using Core.Plugins.Samples.Domain.DTO;

namespace Core.Plugins.Samples.Domain.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            CreateRules();
        }

        private void CreateRules()
        {
            RuleFor(order => order.OrderId).GreaterThan(0);
            RuleFor(order => order.OrderDate).NotEqual(DateTime.MinValue);
        }
    }
}
