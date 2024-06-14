using System;
using HCM.Infrastucture.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace HCM.Infrastucture.Middlewares
{
    public static class InfraMiddlewareExtension
    {
        public static void DbContextInitializer(this IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<HCMDbContextInitializer>();
                initialiser.Initialize();
                initialiser.Seed();
            }
        }
    }
}
