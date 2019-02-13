using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class JsonConverterProvider : IJsonConverterProvider
    {
        public IList<JsonConverter> GetJsonConverters()
        {
            return new List<JsonConverter>
            {
                new BitstringConverter(),
                new DomainConverter(),
                new BigIntegerConverter(),
                new StringEnumConverter()
            };
        }
    }
}