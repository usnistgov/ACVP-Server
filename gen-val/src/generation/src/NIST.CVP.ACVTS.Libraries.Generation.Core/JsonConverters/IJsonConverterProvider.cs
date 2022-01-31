using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters
{
    public interface IJsonConverterProvider
    {
        IEnumerable<JsonConverter> GetJsonConverters();
    }
}
