using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore] public ITestCaseExpectationReason<SigFailureReasons> Reason { get; set; }
        [JsonProperty(PropertyName = "reason")]
        public string ReasonVal => Reason?.GetName();

        /// <summary>
        /// Ignoring for (De)Serialization as KeyPairs are flattened
        /// </summary>
        [JsonIgnore] public FfcKeyPair Key { get; set; } = new FfcKeyPair();
        [JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger X
        {
            get => Key.PrivateKeyX;
            set => Key.PrivateKeyX = value;
        }
        [JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Y
        {
            get => Key.PublicKeyY;
            set => Key.PublicKeyY = value;
        }

        public BitString Message { get; set; }

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
