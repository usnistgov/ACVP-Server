using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        /// <summary>
        /// Ignoring for (De)Serialization as KeyPairs are flattened
        /// </summary>
        [JsonIgnore]
        public FfcKeyPair Key { get; set; } = new FfcKeyPair();

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

        // Needed for FireHoseTests
        public BigInteger K;

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            var expandoSource = (ExpandoObject) source;

            Message = expandoSource.GetBitStringFromProperty("message");

            BigInteger x, y;
            x = expandoSource.GetBigIntegerFromProperty("x");
            y = expandoSource.GetBigIntegerFromProperty("y");
            Key = new FfcKeyPair(x, y);
            
            BigInteger r, s;
            r = expandoSource.GetBigIntegerFromProperty("r");
            s = expandoSource.GetBigIntegerFromProperty("s");
            Signature = new FfcSignature(r, s);
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "x":
                    Key.PrivateKeyX = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "y":
                    Key.PublicKeyY = new BitString(value).ToPositiveBigInteger();
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

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
