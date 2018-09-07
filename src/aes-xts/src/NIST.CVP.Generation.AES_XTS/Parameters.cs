using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XTS
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public int[] KeyLen { get; set; }
        public string[] Direction { get; set; }
        public MathDomain PtLen { get; set; }
        [JsonProperty("TweakMode")]
        public string[] TweakModes { get; set; }
    }
}
