using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "context")]
        public BitString Context { get; set; }

        [JsonIgnore] public EdSignature Signature { get; set; } = new EdSignature();
        [JsonProperty(PropertyName = "sig", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Sig
        {
            get => Signature.Sig;
            set => Signature.Sig = value;
        }
    }
}
