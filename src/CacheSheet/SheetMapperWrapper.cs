using System;
using System.Collections.Generic;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;

namespace CacheSheet
{
    public class SheetMapperWrapper
    {
        private readonly SheetMapper _sheetMapper;
        private readonly Dictionary<Type, string> _dictionary;

        public SheetMapperWrapper(SheetMapper sheetMapper, Dictionary<Type, string> dictionary)
        {
            _sheetMapper = sheetMapper;
            _dictionary = dictionary;
        }

        public SheetMapperWrapper AddConfigFor<T>(
            Func<MappingConfigBuilder<T>, MappingConfigBuilder<T>> mappingConfigFunc, string range)
            where T : new()
        {
            _dictionary.Add(typeof(T), range);
            return new SheetMapperWrapper(_sheetMapper.AddConfigFor(mappingConfigFunc), _dictionary);
        }

        public string GetRange(Type type)
        {
            return _dictionary[type];
        }

        public MappingResult<T> Map<T>(Sheet sheet) where T : new()
        {
            return _sheetMapper.Map<T>(sheet);
        }
    }
}