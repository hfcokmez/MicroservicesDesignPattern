using System;
using Shared.Abstract;

namespace Shared.Events
{
    public class OrderRequestFailedEvent : IOrderRequestFailedEvent
    {
        public int OrderId { get; set; }
        public string Message { get; set;  }
    }
}
