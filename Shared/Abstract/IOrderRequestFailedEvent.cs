using System;
namespace Shared.Abstract
{
    public interface IOrderRequestFailedEvent
    {
        public int OrderId { get; set; }
        public string Message { get; set; }
    }
}
