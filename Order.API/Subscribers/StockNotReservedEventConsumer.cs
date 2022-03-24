﻿using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Subscribers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StockNotReservedEventConsumer> _logger;

        public StockNotReservedEventConsumer(AppDbContext context, ILogger<StockNotReservedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
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
