using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheSheet.Tests.Interop
{
    public class MockSpreadsheetRepository : DataRepository
    {
        public int CountCall;
        public Task<string[][]> GetAsync(string range)
        {
            CountCall++;
            return Task.FromResult(new[] {new[] {"Hello World!"}});
        }

        public async Task<IEnumerable<T>> LoadAllAsync<T>() where T : new()
        {
            CountCall++;
            return await Task.FromResult(new List<T>());
        }

        public async Task<Dictionary<string, string>> GetDictionaryAsync(string range)
        {
            return await Task.FromResult(new Dictionary<string, string>());
        }
    }
}