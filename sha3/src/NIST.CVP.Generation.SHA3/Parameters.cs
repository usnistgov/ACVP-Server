using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "function")]
        public string[] Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int[] DigestSize { get; set; }

        [JsonProperty(PropertyName = "bitOrientedInput")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "bitOrientedOutput")]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonProperty(PropertyName = "includeNull")]
        public bool IncludeNull { get; set; } = false;

        [JsonProperty(PropertyName = "minOutputLength")]
        public int MinOutputLength { get; set; } = 0;

        [JsonProperty(PropertyName = "maxOutputLength")]
        public int MaxOutputLength { get; set; } = 0;
    }
}
