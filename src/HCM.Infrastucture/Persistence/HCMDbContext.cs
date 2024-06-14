using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Extensions;

namespace HCM.Infrastucture.Persistence
{
    public class HCMDbContext : DbContext
    {
        public HCMDbContext()
        {
        }

        public HCMDbContext(DbContextOptions<HCMDbContext> options) : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetConvetions();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
