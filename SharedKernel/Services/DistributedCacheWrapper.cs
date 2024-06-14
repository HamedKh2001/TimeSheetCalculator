using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Contracts.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Services
{
    public class DistributedCacheWrapper : IDistributedCacheWrapper
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheWrapper(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public string GetString(string key)
        {
            return _distributedCache.GetString(key);
        }

        public Task<string> GetStringAsync(string key, CancellationToken cancellationToken)
        {
            return _distributedCache.GetStringAsync(key, cancellationToken);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            return _distributedCache.RemoveAsync(key);
        }

        public void SetString(string key, string value, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            _distributedCache.SetString(key, value, distributedCacheEntryOptions);
        }

        public Task SetStringAsync(string key, string value, CancellationToken cancellationToken, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            return _distributedCache.SetStringAsync(key, value, distributedCacheEntryOptions);
        }

        public Task SetStringAsync(string key, string value, CancellationToken token = default)
        {
            return _distributedCache.SetStringAsync(key, value, token);
        }
    }
}
