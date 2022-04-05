using System;
namespace Shared.Abstract
{
    public interface IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
