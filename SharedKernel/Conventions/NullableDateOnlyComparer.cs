using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace SharedKernel.Conventions
{
    public class NullableDateOnlyComparer : ValueComparer<DateOnly?>
    {
        public NullableDateOnlyComparer() : base((d1, d2) => d1 == d2 && d1.GetValueOrDefault().DayNumber == d2.GetValueOrDefault().DayNumber, d => d.GetHashCode())
        {
        }
    }
}
