using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace SharedKernel.Conventions
{
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(d => d.ToDateTime(TimeOnly.MinValue), d => DateOnly.FromDateTime(d)) { }
    }
}
