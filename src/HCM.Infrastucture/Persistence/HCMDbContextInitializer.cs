using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HCM.Infrastucture.Persistence
{
    public class HCMDbContextInitializer
    {
        private readonly ILogger<HCMDbContextInitializer> _logger;
        private readonly HCMDbContext _context;

        public HCMDbContextInitializer(ILogger<HCMDbContextInitializer> logger, HCMDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                _context.Database.Migrate();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public void Seed()
        {
            try
            {
                TrySeed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public void TrySeed()
        {

        }
    }
}
