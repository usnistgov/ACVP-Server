using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
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
        public BitString D
        {
            get => KeyPair?.PrivateD?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => KeyPair?.PublicQ?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PublicQ = value;
        }

        [JsonProperty(PropertyName = "preHash")]
        public bool PreHash { get; set; }

        [JsonIgnore]
        public BitString Message { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        public MathDomain ContextLength { get; set; }
        public BitString Context { get; set; }
    }
}
