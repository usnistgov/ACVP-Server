using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore] public bool? TestPassed => true;
        [JsonIgnore] public bool Deferred => true;

        private int l => ParentGroup?.DomainParameterL ?? 0;
        private int n => ParentGroup?.DomainParameterN ?? 0;

        /// <summary>
        /// Ignoring for (De)Serialization as KeyPairs are flattened
        /// </summary>
        [JsonIgnore]
        public FfcKeyPair Key { get; set; } = new FfcKeyPair();

        [JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString X
        {
            get => Key.PrivateKeyX != 0 ? new BitString(Key.PrivateKeyX, n) : null;
            set => Key.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Y
        {
            get => Key.PublicKeyY != 0 ? new BitString(Key.PublicKeyY, l) : null;
            set => Key.PublicKeyY = value.ToPositiveBigInteger();
        }
    }
}
