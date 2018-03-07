using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core.JsonConverters
{
    public interface IJsonConverterProvider
    {
        IEnumerable<JsonConverter> GetJsonConverters();
    }
}