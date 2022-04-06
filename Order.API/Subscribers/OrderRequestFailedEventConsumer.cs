using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.API.Models;
using Shared.Abstract;

namespace Order.API.Subscribers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRequestFailedEventConsumer> _logger;

        public OrderRequestFailedEventConsumer(AppDbContext context, ILogger<OrderRequestFailedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMassage = context.Message.Message;
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
