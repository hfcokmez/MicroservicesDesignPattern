using System;
using System.Collections.Generic;

namespace Shared.Messages
{
    public class StockRollbackMessage
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
