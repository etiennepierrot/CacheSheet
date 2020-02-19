using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace CacheSheet
{
    public class CachedDataRepository : DataRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _expiration;

        public CachedDataRepository(DataRepository dataRepository, IMemoryCache memoryCache, TimeSpan expiration)
        {
            _dataRepository = dataRepository;
            _memoryCache = memoryCache;
            _expiration = expiration;
        }

        public Task<string[][]> GetAsync(string range)
        {
            return _memoryCache.GetOrCreate(range,
                entry =>
                {
                    entry.AbsoluteExpiration = EntryAbsoluteExpiration();
                    return _dataRepository.GetAsync(range);
                }
            );
        }

       
        public async Task<IEnumerable<T>> LoadAllAsync<T>() where T : new()
        {
            return  await _memoryCache.GetOrCreate(typeof(T), async entry =>
                {
                    entry.AbsoluteExpiration = EntryAbsoluteExpiration();
                    return await _dataRepository.LoadAllAsync<T>();
                }
            );
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(string range)
        {
            return await _memoryCache.GetOrCreate(range,
                entry =>
                {
                    entry.AbsoluteExpiration = EntryAbsoluteExpiration();
                    return _dataRepository.GetDictionaryAsync(range);
                }
            );
        }

        private DateTime EntryAbsoluteExpiration()
        {
            return DateTime.Now.Add(_expiration);
        }

    }
}