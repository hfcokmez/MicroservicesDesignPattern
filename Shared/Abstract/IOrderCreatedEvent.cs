using System;
using System.Collections.Generic;
using MassTransit;
using Shared.Messages;

namespace Shared.Abstract
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; set; }
        Guid CorrelationId { get; }
    }
}
