using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Pools.Interfaces
{
    public interface IJsonConverterProvider
    {
        IList<JsonConverter> GetJsonConverters();
    }
}