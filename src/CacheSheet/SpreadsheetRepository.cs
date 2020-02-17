using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SheetToObjects.Adapters.ProtectedGoogleSheets;
using SheetToObjects.Lib;

namespace CacheSheet
{
    
    public class SpreadsheetRepository : DataRepository
    {
        private readonly string _spreadSheetId;
        private readonly string _pathCredential;
        private readonly IProvideSheet _adapter;
        private readonly ConfigMapper _configMapper;

        public SpreadsheetRepository(string spreadSheetId, string pathCredential, ConfigMapper configMapper)
        {
            _spreadSheetId = spreadSheetId;
            _pathCredential = pathCredential;
            _configMapper = configMapper;
            _adapter = new ProtectedGoogleSheetAdapter();
        }
        

        public async Task<string[][]> GetAsync(string range)
        {
            var sheet = await ReadSheetAsync(range);
            return sheet.Rows
                .Select(v => v.Cells.Select(c => c.Value.ToString()).ToArray())
                .ToArray();
        }
        
        public async Task<Dictionary<string, string>> GetDictionaryAsync(string range)
        {
            var sheet = await ReadSheetAsync(range);
            return sheet.Rows.ToDictionary(
                x => x.Cells[0].Value.ToString(), 
                x => x.Cells[1].Value.ToString());
        }

        public async Task<IEnumerable<T>> LoadAllAsync<T>() where T : new()
        {
            string range = _configMapper.ConfigRange[typeof(T)];
            var sheet = await ReadSheetAsync(range);
            return _configMapper.SheetMapper.Map<T>(sheet)
                .ParsedModels.Select(m => m.Value);
        }

        private async Task<Sheet> ReadSheetAsync(string range)
        {
            return await _adapter.GetAsync(_pathCredential,null, _spreadSheetId,
                range);
        }
    }

  
}