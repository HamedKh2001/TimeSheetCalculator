using Microsoft.EntityFrameworkCore;
using SharedKernel.Conventions;
using System;

namespace SharedKernel.Extensions
{
    public static class ConventionExtensions
    {
        public static void SetConvetions(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter, DateOnlyComparer>();
            configurationBuilder.Properties<DateOnly?>().HaveConversion<NullableDateOnlyConverter, NullableDateOnlyComparer>();
        }
    }
}
