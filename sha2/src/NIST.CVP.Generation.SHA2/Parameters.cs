using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;

namespace NIST.CVP.Generation.SHA2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "function")]
        public string[] Mode { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public string[] DigestSize { get; set; }

        [JsonProperty(PropertyName = "bitOriented")]
        public bool BitOriented { get; set; } = false;

        [JsonProperty(PropertyName = "includeNull")]
        public bool IncludeNull { get; set; } = false;
    }
}
