﻿using System;
using System.Collections.Generic;
using Shared.Messages;

namespace Shared.Events
{
    public class PaymentFailedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public string Message { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
