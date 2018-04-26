using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public SigFailureReasons Reason { get; set; }
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

        // For FireHoseTests
        public EccKeyPair KeyPair = new EccKeyPair();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (value.Length % 2 != 0)
            {
                value = "0" + value;
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "qx":
                    KeyPair.PublicQ.X = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qy":
                    KeyPair.PublicQ.Y = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "r":
                    Signature.R = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    Signature.S = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "result":
                    TestPassed = value.ToLower()[0] == 'p';
                    return true;
            }

            return false;
        }
    }
}
