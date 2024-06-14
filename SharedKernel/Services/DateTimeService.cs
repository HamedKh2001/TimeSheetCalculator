using SharedKernel.Contracts.Infrastructure;
using System;

namespace SharedKernel.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime MinValue => DateTime.MinValue;
    }
}
