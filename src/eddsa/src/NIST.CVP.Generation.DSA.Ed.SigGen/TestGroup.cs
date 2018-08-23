using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }

        [JsonIgnore] public EdKeyPair KeyPair { get; set; } = new EdKeyPair();
        [JsonProperty(PropertyName = "d", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger D
        {
            get => KeyPair.PrivateD;
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Q
        {
            get => KeyPair.PublicQ;
            set => KeyPair.PublicQ = value;
        }

        [JsonProperty(PropertyName = "preHash")]
        public bool PreHash { get; set; }

        [JsonIgnore]
        public BitString Message { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
