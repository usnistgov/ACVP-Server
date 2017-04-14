using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.SHA2;

namespace NIST.CVP.Generation.SHA2
{
    public class Function
    {
        [JsonProperty(PropertyName = "type")]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "digestSizes")]
        public string[] DigestSizes { get; set; }
    }
}
