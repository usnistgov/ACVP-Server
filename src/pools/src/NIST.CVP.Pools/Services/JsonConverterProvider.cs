using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Math.JsonConverters;
using NIST.CVP.Pools.Interfaces;
using System.Collections.Generic;

namespace NIST.CVP.Pools.Services
{
    public class JsonConverterProvider : IJsonConverterProvider
    {
        public IList<JsonConverter> GetJsonConverters()
        {
            return new List<JsonConverter>
            {
                new BitstringBitLengthConverter(),
                new DomainConverter(),
                new BigIntegerConverter(),
                new StringEnumConverter()
            };
        }
    }
}