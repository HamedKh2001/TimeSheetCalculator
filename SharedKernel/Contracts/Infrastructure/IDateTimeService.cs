using System;

namespace SharedKernel.Contracts.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
        DateTime MinValue { get; }
    }
}
