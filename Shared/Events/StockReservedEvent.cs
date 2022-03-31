using System;
using System.Collections.Generic;
using MassTransit;
using Shared.Abstract;
using Shared.Messages;

namespace Shared.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public StockReservedEvent(Guid CorrelationId)
        {
            this.CorrelationId = CorrelationId;
        }
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get;  }
    }
}
