using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastucture;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CreateOrderCommandMessageConsumer : IConsumer<CreateOrderCommandMessage>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderCommandMessageConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderCommandMessage> context)
        {
            var newAdress = new Address(
                context.Message.Province, context.Message.District, context.Message.Street,
                context.Message.ZipCode, context.Message.Line);

            var order = new Order.Domain.OrderAggregate.Order(context.Message.BuyerId, newAdress);

            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
            });

            await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();

        }
    }
}
