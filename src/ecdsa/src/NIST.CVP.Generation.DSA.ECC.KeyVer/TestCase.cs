using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        public EcdsaKeyDisposition Reason { get; set; }

        [JsonIgnore] public EccKeyPair KeyPair { get; set; } = new EccKeyPair();
        [JsonProperty(PropertyName = "d", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger D
        {
            get => KeyPair.PrivateD;
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "qx", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Qx
        {
            get => KeyPair.PublicQ.X;
            set => KeyPair.PublicQ.X = value;
        }

        [JsonProperty(PropertyName = "qy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Qy
        {
            get => KeyPair.PublicQ.Y;
            set => KeyPair.PublicQ.Y = value;
        }

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
                case "qx":
                    KeyPair.PublicQ.X = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qy":
                    KeyPair.PublicQ.Y = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "result":
                    TestPassed = value.Contains("p (0");
                    return true;
            }

            return false;
        }
    }
}
