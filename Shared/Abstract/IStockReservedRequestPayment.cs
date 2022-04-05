using System;
using System.Collections.Generic;
using MassTransit;
using Shared.Messages;

namespace Shared.Abstract
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage > OrderItems { get; set; }
    }
}
