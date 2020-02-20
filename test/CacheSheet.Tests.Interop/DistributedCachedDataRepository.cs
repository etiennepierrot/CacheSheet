using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CacheSheet.Tests.Interop
{
    public class DistributedCachedDataRepository : DataRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly TimeSpan _expiration;

        public DistributedCachedDataRepository(
            DataRepository dataRepository, 
            IDistributedCache distributedCache, TimeSpan expiration)
        {
            _dataRepository = dataRepository;
            _distributedCache = distributedCache;
            _expiration = expiration;
        }

        public async Task<string[][]> GetAsync(string range)
        {
            return await Cache(key => _dataRepository.GetAsync(key), range);
        }
        
        public async Task<IEnumerable<T>> LoadAllAsync<T>() where T : new()
        {
            return await Cache(key => _dataRepository.LoadAllAsync<T>(), typeof(T).ToString());
        }
        
        public async Task<Dictionary<string, string>> GetDictionaryAsync(string range)
        {
            return await Cache(key => _dataRepository.GetDictionaryAsync(key), range);
        }
        

        private async Task<T> Cache<T>(Func<string, Task<T>>func, string key)
        {
            string data = await _distributedCache.GetStringAsync(key);
            if (data != null) return JsonConvert.DeserializeObject<T>(data);
            var result = await func(key);
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(result), 
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = _expiration,
                });
            return result;
        }
    }
}