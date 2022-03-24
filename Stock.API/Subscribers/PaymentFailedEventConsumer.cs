using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Events;
using Stock.API.Models;

namespace Stock.API.Subscribers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentFailedEventConsumer> _logger;

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (stock != null)
                {
                    stock.Count -= item.Count;
                    await _context.SaveChangesAsync();
                }
            }
            _logger.LogInformation($"Stock was released for Order Id: {context.Message.OrderId}");
        }
    }
}
