using System;
using System.Collections.Generic;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;

namespace CacheSheet.Tests.Interop
{
    public class SheetMapperFactory
    {
        public static SheetMapperWrapper BuildSheetMapperWrapper()
        {
            return new SheetMapperWrapper(new SheetMapper(), new Dictionary<Type, string>())
                .AddConfigFor<User>(cfg =>
                {
                    return cfg
                        .MapColumn(column => column.WithHeader("User Name").IsRequired()
                            .MapTo(m => m.Username))
                        .MapColumn(column => column.WithHeader("Age").IsRequired()
                            .MapTo(m => m.Age));
                }, "Users");
        }

    }
}