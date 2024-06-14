using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Contracts.Infrastructure
{
    public interface IDistributedCacheWrapper
    {
        Task RemoveAsync(string key);
        void Remove(string key);
        string GetString(string key);
        Task<string> GetStringAsync(string key, CancellationToken cancellationToken);
        void SetString(string key, string value, DistributedCacheEntryOptions distributedCacheEntryOptions);
        Task SetStringAsync(string key, string value, CancellationToken cancellationToken, DistributedCacheEntryOptions distributedCacheEntryOptions);
        Task SetStringAsync(string key, string value, CancellationToken token = default);
    }
}
