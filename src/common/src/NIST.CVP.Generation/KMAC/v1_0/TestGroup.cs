﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        public string TestType { get; set; }

        public MathDomain KeyLengths { get; set; }
        
        [JsonProperty(PropertyName = "xof")]
        public bool XOF { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }
        
        public int MessageLength { get; set; }

        public MathDomain MsgLengths { get; set; }

        public MathDomain MacLengths { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore]
        public bool HexCustomization { get; set; } = false;
    }
}