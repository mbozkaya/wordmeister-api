using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.EntityExtended;

namespace wordmeister_api.Helpers
{
    public sealed class Converter : ValueConverter<string, string>
    {
        public Converter(ConverterMappingHints mappingHints = null)
            : base(x => Tools.AesEncrypt(x), x => Tools.AesDecrypt(x), mappingHints)
        {
        }
    }
}
