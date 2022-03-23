using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Payment.API.Subscriber
{
    public class StockReservedEventSubscriber : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventSubscriber> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventSubscriber(ILogger<StockReservedEventSubscriber> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;
            if(balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for User Id: {context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentSuccessedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL wasn't withdrawn from credit card for User Id: {context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentFailedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId, Message = "Not enough balance." });
            }
        }
    }
}
