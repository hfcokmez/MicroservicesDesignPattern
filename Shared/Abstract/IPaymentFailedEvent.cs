using System;
using System.Collections.Generic;
using MassTransit;
using Shared.Messages;

namespace Shared.Abstract
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Message { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
