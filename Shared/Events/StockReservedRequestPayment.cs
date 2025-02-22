﻿using System;
using System.Collections.Generic;
using Shared.Abstract;
using Shared.Messages;

namespace Shared.Events
{
    public class StockReservedRequestPayment : IStockReservedRequestPayment
    {
        public StockReservedRequestPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get; }
        public string BuyerId { get; set; }
    }
}
