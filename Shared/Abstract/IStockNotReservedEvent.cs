using System;
using MassTransit;

namespace Shared.Abstract
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        string Message { get; set; }   
    }
}
