using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Event;
using Shared.Events;
using Shared.Settings;
using Stock.API.Models;

namespace Stock.API.Subscribers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();

            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await _context.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }
            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }
                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation($"Stock was reserver for Buyer Id: {context.Message.BuyerId}");
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue: {RabbitMQSettings.StockReservedEventQueueName}"));
                var stockReservedEvent = new StockReservedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    Payment = context.Message.PaymentMessage,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems
                };
                await _sendEndpointProvider.Send(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent() {
                    OrderId = context.Message.OrderId,
                    Message = "Not enough stock"
                });
                _logger.LogInformation($"Not enough stock for Buyer Id: {context.Message.BuyerId}");
            }
        }
    }
}
