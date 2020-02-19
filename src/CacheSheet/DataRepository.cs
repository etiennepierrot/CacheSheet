using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheSheet
{
    public interface DataRepository
    {
        Task<string[][]> GetAsync(string range);
        Task<IEnumerable<T>> LoadAllAsync<T>() where T : new();
        Task<Dictionary<string,string>> GetDictionaryAsync(string range);
    }
}