using System;
using System.Collections.Generic;
using Shared.Messages;

namespace Shared.Abstract
{
    public interface IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public PaymentMessage Payment { get; set; }
    }
}
