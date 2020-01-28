using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;

namespace NIST.CVP.Generation.KAS_IFC.Sp800_56Br2
{
    public class KasJsonConverterProvider : JsonConverterProvider
    {
        protected override void AddAdditionalConverters(HashSet<JsonConverter> registeredConverters)
        {
            registeredConverters.Add(new KdfConfigurationConverter());
            registeredConverters.Add(new KdfParameterConverter());
        }
    }
}