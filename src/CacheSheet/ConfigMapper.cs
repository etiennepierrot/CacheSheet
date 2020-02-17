using System;
using System.Collections.Generic;
using SheetToObjects.Lib;

namespace CacheSheet
{
    public class ConfigMapper
        {
            public SheetMapper SheetMapper { get; set; }
            public Dictionary<Type, string> ConfigRange { get; set; }
        }
}