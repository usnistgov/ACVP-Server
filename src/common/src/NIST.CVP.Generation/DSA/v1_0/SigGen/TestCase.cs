using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Generation.DSA.v1_0.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        /// <summary>
        /// Note key is used only for firehose tests, key is a group level property for current testing
        /// </summary>
        [JsonIgnore]
        public FfcKeyPair Key { get; set; } = new FfcKeyPair();

        public BitString Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString RandomValue { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RandomValueLen { get; set; }

        [JsonIgnore] public FfcSignature Signature { get; set; } = new FfcSignature();
        [JsonProperty(PropertyName = "r", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString R
        {
            get => Signature.R;
            set => Signature.R = value;
        }
        [JsonProperty(PropertyName = "s", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString S
        {
            get => Signature.S;
            set => Signature.S = value;
        }

        // Needed for FireHoseTests
        public BigInteger K;

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "x":
                    Key.PrivateKeyX = new BitString(value);
                    return true;

                case "y":
                    Key.PublicKeyY = new BitString(value);
                    return true;

                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    Signature.R = new BitString(value);
                    return true;

                case "s":
                    Signature.S = new BitString(value);
                    return true;

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
