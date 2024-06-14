using System.Threading.Tasks;

namespace SharedKernel.Contracts.Infrastructure
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task<int> SaveChangesAsync();
        Task SaveAndCommitAsync();
        void ClearChangeTracker();
    }
}
