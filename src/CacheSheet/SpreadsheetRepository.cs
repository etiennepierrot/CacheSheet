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
        private readonly ProtectedGoogleSheetAdapter _adapter;
        private readonly SheetMapperWrapper _sheetMapperWrapper;

        public SpreadsheetRepository(string spreadSheetId, string pathCredential, SheetMapperWrapper sheetMapperWrapper)
        {
            _spreadSheetId = spreadSheetId;
            _pathCredential = pathCredential;
            _sheetMapperWrapper = sheetMapperWrapper;
            _adapter = new ProtectedGoogleSheetAdapter();
        }

        public async Task<string[][]> GetAsync(string range)
        {
            var result = await ReadSheetAsync(range);
            return result.Rows
                .Select(v => v.Cells.Select(c => c.Value.ToString()).ToArray())
                .ToArray();
        }

        public async Task<IEnumerable<T>> LoadAllAsync<T>() where T : new()
        {
            string range = _sheetMapperWrapper.GetRange(typeof(T));
            var result = await ReadSheetAsync(range);
            return _sheetMapperWrapper.Map<T>(result)
                .ParsedModels.Select(m => m.Value);
        }

        private async Task<Sheet> ReadSheetAsync(string range)
        {
            return await _adapter.GetAsync(_pathCredential,null, _spreadSheetId,
                range);
        }
    }
}