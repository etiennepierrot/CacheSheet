using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using Xunit;
using MemoryCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace CacheSheet.Tests.Interop
{
    public class CacheSheetTest
    {
        private readonly CachedDataRepository _cachedMockedDataRepository;
        private readonly CachedDataRepository _cachedDataRepository;
        private readonly MockSpreadsheetRepository _mockRepository;
        private readonly SpreadsheetRepository _spreadsheetRepository;

        public CacheSheetTest()
        {
            _spreadsheetRepository = ServiceFactory.CreateSpreadSheetRepository();
            _mockRepository = ServiceFactory.CreateMockSpreadsheetRepository();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _cachedMockedDataRepository = new CachedDataRepository(_mockRepository, memoryCache, TimeSpan.FromSeconds(1));
            _cachedDataRepository = new CachedDataRepository(_spreadsheetRepository, memoryCache, TimeSpan.FromSeconds(1));
        }
        [Fact]
        public async Task HelloWorld_From_Sheet()
        {
            SheetCache sheetCache = new SheetCache(_spreadsheetRepository);
            var csvString = await sheetCache.GetCSVStringAsync("Test!A1");
            csvString.Should().Be("Hello World!");
        }

        [Fact]
        public async Task MapObject_From_Sheet()
        {
            SheetCache sheetCache = new SheetCache(_spreadsheetRepository);
            var users = await sheetCache.Get<User>();

            users.Single().Should().BeEquivalentTo(new User
            {
                Age = 38,
                Username = "Etienne"
            });
        }

        [Fact]
        public async Task MapObject_From_SheetCache()
        {
            SheetCache sheetCache = new SheetCache(_cachedDataRepository);
            var user1 = await sheetCache.Get<User>();
            var users2 = await sheetCache.Get<User>();
            users2.Single().Should().BeEquivalentTo(user1.Single());
        }
        
        [Fact]
        public async Task MapDictionary_From_Sheet()
        {
            SheetCache sheetCache = new SheetCache(_spreadsheetRepository);
            Dictionary<string, string> dictionary = await sheetCache.GetDictionary("Dictionary");
            
            dictionary.Should().BeEquivalentTo(new Dictionary<string, string>()
            {
                {"fizz", "buzz"},
                {"foo", "bar"}
            });
        }

        
        [Fact]
        public async Task HelloWorld_From_Cache()
        {
            SheetCache sheetCache = new SheetCache(_cachedMockedDataRepository);
            await sheetCache.GetCSVStringAsync("Test!A1");
            await sheetCache.GetCSVStringAsync("Test!A1");
            _mockRepository.CountCall.Should().Be(1);
        }

        [Fact]
        public async Task Cache_Should_Expire()
        {
            SheetCache sheetCache = new SheetCache(_cachedMockedDataRepository);
            await sheetCache.GetCSVStringAsync("Test!A1");
            Thread.Sleep(TimeSpan.FromSeconds(1.1));
            await sheetCache.GetCSVStringAsync("Test!A1");
            _mockRepository.CountCall.Should().Be(2);
        }
    }
}