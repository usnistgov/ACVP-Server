using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonIgnore] public EccSignature Signature { get; set; } = new EccSignature();
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

        // Needed for FireHoseTests
        public EccKeyPair KeyPair;
        public BigInteger K;
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Sometimes these values aren't even length...
            if (value.Length % 2 != 0)
            {
                value = value.Insert(0, "0");
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    Signature.R = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    Signature.S = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "d":
                    KeyPair = new EccKeyPair(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
