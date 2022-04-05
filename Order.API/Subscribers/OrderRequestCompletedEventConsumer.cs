using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.API.Models;
using Shared.Abstract;

namespace Order.API.Subscribers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRequestCompletedEventConsumer> _logger;

        public OrderRequestCompletedEventConsumer(AppDbContext context, ILogger<OrderRequestCompletedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Success;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order (Id: {context.Message.OrderId}) Status Changed: {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id: {context.Message.OrderId}) not found.");
            }
        }
    }
}
