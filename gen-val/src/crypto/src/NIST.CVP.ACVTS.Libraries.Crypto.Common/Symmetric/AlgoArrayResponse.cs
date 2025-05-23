﻿using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric
{
    public class AlgoArrayResponse : IAlgoArrayResponse
    {
        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }
        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }
    }
}
