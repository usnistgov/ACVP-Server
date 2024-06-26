﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM_SIV.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; } = "AFT";

        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        [JsonProperty(PropertyName = "payloadLen")]
        public int PayloadLength { get; set; }

        [JsonProperty(PropertyName = "aadLen")]
        public int AadLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }


}
