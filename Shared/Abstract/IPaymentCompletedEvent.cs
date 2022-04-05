using System;
using MassTransit;

namespace Shared.Abstract
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}
