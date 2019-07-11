using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Generation.DSA.v1_0.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        private int l => ParentGroup?.L ?? 0;
        private int n => ParentGroup?.N ?? 0;
        
        [JsonIgnore]
        public ITestCaseExpectationReason<DsaSignatureDisposition> Reason { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string ReasonVal
        {
            get => Reason?.GetName();

            set
            {
                var failureReason = EnumHelpers.GetEnumFromEnumDescription<DsaSignatureDisposition>(value);
                Reason = new TestCaseExpectationReason(failureReason);
            }
        }

        /// <summary>
        /// Ignoring for (De)Serialization as KeyPairs are flattened
        /// </summary>
        [JsonIgnore] public FfcKeyPair Key { get; set; } = new FfcKeyPair();
        [JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString X
        {
            get => Key?.PrivateKeyX != 0 ? new BitString(Key.PrivateKeyX, n) : null;
            set => Key.PrivateKeyX = value.ToPositiveBigInteger();
        }
        [JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Y
        {
            get => Key?.PublicKeyY != 0 ? new BitString(Key.PublicKeyY, l) : null;
            set => Key.PublicKeyY = value.ToPositiveBigInteger();
        }

        public BitString Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString RandomValue { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RandomValueLen { get; set; }

        [JsonIgnore] public FfcSignature Signature { get; set; } = new FfcSignature();
        [JsonProperty(PropertyName = "r", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger R
        {
            get => Signature.R;
            set => Signature.R = value;
        }
        [JsonProperty(PropertyName = "s", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger S
        {
            get => Signature.S;
            set => Signature.S = value;
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "y":
                    Key = new FfcKeyPair(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    Signature.R = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    Signature.S = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "result":
                    TestPassed = value.ToLower()[0] != 'f';
                    return true;
            }

            return false;
        }
    }
}
