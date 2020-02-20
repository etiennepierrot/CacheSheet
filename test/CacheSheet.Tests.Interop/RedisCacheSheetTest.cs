using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CacheSheet.Tests.Interop
{
    public class RedisCacheSheetTest
    {
        private readonly DistributedCachedDataRepository _cachedDataRepository;
        private readonly DistributedCachedDataRepository _mockedCachedDataRepository;
        private readonly MockSpreadsheetRepository _mockRepository;

        public RedisCacheSheetTest()
        {
            _mockRepository = ServiceFactory.CreateMockSpreadsheetRepository();
            var spreadSheetRepository = ServiceFactory.CreateSpreadSheetRepository();
            var expiration = TimeSpan.FromSeconds(1);
            _mockedCachedDataRepository = ServiceFactory.CreateDistributedCachedDataRepository(expiration, _mockRepository);
            _cachedDataRepository = ServiceFactory.RedisCachedDataRepository(expiration, spreadSheetRepository);
        }
        
        [Fact]
        public async Task HelloWorld_From_Cache()
        {
            SheetCache sheetCache = new SheetCache(_mockedCachedDataRepository);
            await sheetCache.GetCSVStringAsync("Test!A1");
            await sheetCache.GetCSVStringAsync("Test!A1");
            _mockRepository.CountCall.Should().Be(1);
        } 
        
        [Fact]
        public async Task MapObject_From_Sheet()
        {
            SheetCache sheetCache = new SheetCache(_cachedDataRepository);
            var users = await sheetCache.Get<User>();

            users.Single().Should().BeEquivalentTo(new User
            {
                Age = 38,
                Username = "Etienne"
            });
        }
    }
}