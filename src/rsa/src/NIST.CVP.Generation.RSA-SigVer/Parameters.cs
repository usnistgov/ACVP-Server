﻿using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "algSpecs")]
        public AlgSpecs[] Capabilities { get; set; }
        public string PubExpMode { get; set; }

        [JsonProperty(PropertyName = "fixedPubExp")]
        public string FixedPubExpValue { get; set; } = "";

        [JsonIgnore]
        public string KeyFormat { get; set; } = "standard";
    }

    public class AlgSpecs
    {
        public string SigType { get; set; }

        [JsonProperty(PropertyName = "sigTypeCapabilities")]
        public CapSigType[] ModuloCapabilities { get; set; }
    }

    public class CapSigType
    {
        public int Modulo { get; set; }

        [JsonProperty(PropertyName = "hashPair")]
        public HashPair[] HashPairs { get; set; }
    }

    public class HashPair
    {
        public string HashAlg { get; set; }
        public int SaltLen { get; set; } = 0;
    }
}