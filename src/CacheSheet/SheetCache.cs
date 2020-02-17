using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheSheet
{
    public class SheetCache
    {
        private readonly DataRepository _dataRepository;

        public SheetCache(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<string> GetCSVStringAsync(string range)
        {
            return (await _dataRepository.GetAsync(range))
                .Select(x => x.Aggregate((s1, s2) => s1 + ";" + s2))
                .Aggregate( (s1, s2) => s1 + "\r\n" + s2)
                .ToString();
        }

        public async Task<IEnumerable<T>> Get<T>() where T : new()
        {
            return await _dataRepository.LoadAllAsync<T>();
        }
    }
}