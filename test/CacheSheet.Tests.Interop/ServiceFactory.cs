using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;

namespace CacheSheet.Tests.Interop
{
    public static class ServiceFactory
    {
        public static SpreadsheetRepository CreateSpreadSheetRepository()
        {
            return new SpreadsheetRepository(
                "1bQSXT__TWOlymomJ_XplrrWcLF2pCcoUFiCfQ2VHD-g",
                "cachesheet-accountservice.json",
                SheetMapperFactory.BuildSheetMapperWrapper()
            );
        }

        public static MockSpreadsheetRepository CreateMockSpreadsheetRepository()
        {
            return new MockSpreadsheetRepository();
        }

        public static DistributedCachedDataRepository CreateDistributedCachedDataRepository(TimeSpan expiration, DataRepository dataRepository)
        {
            return DistributedCachedDataRepository(expiration, dataRepository);
        }
        
        private static DistributedCachedDataRepository DistributedCachedDataRepository(TimeSpan expiration,
            DataRepository dataRepository)
        {
            IDistributedCache memoryDistributedCache =
                new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            return new DistributedCachedDataRepository(dataRepository, memoryDistributedCache, expiration);
        }

        public static DistributedCachedDataRepository RedisCachedDataRepository(TimeSpan expiration,
            DataRepository dataRepository)
        {
            var redisCache = new RedisCache(Options.Create( new RedisCacheOptions()
            {
                Configuration = "redis_image:6379,abortConnect=False",
            }));
            return new DistributedCachedDataRepository(dataRepository, redisCache, expiration);
        }
    }
}