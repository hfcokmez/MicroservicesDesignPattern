using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Messages;
using Stock.API.Models;

namespace Stock.API.Subscribers
{
    public class StockRollBackMessageConsumer : IConsumer<StockRollbackMessage>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StockRollBackMessageConsumer> _logger;

        public StockRollBackMessageConsumer(AppDbContext context, ILogger<StockRollBackMessageConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<StockRollbackMessage> context)
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
            _logger.LogInformation("Stock was released.");
        }
    }
}
