using System;
using System.Collections.Generic;
using MassTransit;
using Shared.Abstract;
using Shared.Messages;

namespace Shared.Event
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public OrderCreatedEvent(Guid CorrelationId)
        {
            this.CorrelationId = CorrelationId;
        }
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get; }
    }
}
