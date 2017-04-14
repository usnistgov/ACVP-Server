using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.SHA3
{
    public class Function
    {
        [JsonProperty(PropertyName = "type")]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "digestSizes")]
        public int[] DigestSizes { get; set; }
    }
}
