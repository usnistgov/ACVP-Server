﻿using System;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = Array.Empty<string>();
        public PrivateKeyModes[] KeyFormat { get; set; }
        
        [JsonProperty(PropertyName = "modulo")]
        public int[] Modulo { get; set; }
        
        [JsonProperty(PropertyName = "pubExpMode")]
        public PublicExponentModes PublicExponentMode { get; set; }
        
        [JsonProperty(PropertyName = "fixedPubExp")]
        public BitString PublicExponentValue = null;
    }
}
