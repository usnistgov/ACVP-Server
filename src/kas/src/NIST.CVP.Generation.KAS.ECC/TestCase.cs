using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCase : TestCaseBase<TestGroup, TestCase, KasDsaAlgoAttributesEcc>
    {
        [JsonProperty(PropertyName = "staticPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPrivateKeyServer { get; set; }

        [JsonProperty(PropertyName = "staticPublicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPublicKeyServerX { get; set; }

        [JsonProperty(PropertyName = "staticPublicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPublicKeyServerY { get; set; }

        [JsonProperty(PropertyName = "ephemeralPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPrivateKeyServer { get; set; }

        [JsonProperty(PropertyName = "ephemeralPublicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPublicKeyServerX { get; set; }

        [JsonProperty(PropertyName = "ephemeralPublicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPublicKeyServerY { get; set; }


        [JsonProperty(PropertyName = "staticPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPrivateKeyIut { get; set; }

        [JsonProperty(PropertyName = "staticPublicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPublicKeyIutX { get; set; }

        [JsonProperty(PropertyName = "staticPublicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPublicKeyIutY { get; set; }

        [JsonProperty(PropertyName = "ephemeralPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPrivateKeyIut { get; set; }

        [JsonProperty(PropertyName = "ephemeralPublicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPublicKeyIutX { get; set; }

        [JsonProperty(PropertyName = "ephemeralPublicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPublicKeyIutY { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "count":
                    int.TryParse(value, out var result);
                    TestCaseId = result;
                    return true;
                case "dscavs":
                    StaticPrivateKeyServer = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qscavsx":
                    StaticPublicKeyServerX = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qscavsy":
                    StaticPublicKeyServerY = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "decavs":
                    EphemeralPrivateKeyServer = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qecavsx":
                    EphemeralPublicKeyServerX = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qecavsy":
                    EphemeralPublicKeyServerY = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "noncedkmcavs":
                    DkmNonceServer = new BitString(value);
                    break;
                case "nonceephemcavs":
                    EphemeralNonceServer = new BitString(value);
                    break;
                case "dsiut":
                    StaticPrivateKeyIut = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qsiutx":
                    StaticPublicKeyIutX = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qsiuty":
                    StaticPublicKeyIutY = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "deiut":
                    EphemeralPrivateKeyIut = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qeiutx":
                    EphemeralPublicKeyIutX = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "qeiuty":
                    EphemeralPublicKeyIutY = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "noncedkmiut":
                    DkmNonceIut = new BitString(value);
                    break;
                case "nonceephemiut":
                    EphemeralNonceIut = new BitString(value);
                    break;
                case "nonce":
                    NonceNoKc = new BitString(value);
                    return true;
                case "ccmnonce":
                    NonceAesCcm = new BitString(value);
                    return true;
                case "oi":
                    OtherInfo = new BitString(value);
                    OiLen = OtherInfo.BitLength;
                    return true;
                case "cavstag":
                    Tag = new BitString(value);
                    return true;
                case "cavshashzz":
                    HashZ = new BitString(value);
                    return true;
                case "z":
                    Z = new BitString(value);
                    return true;
                case "macdata":
                    MacData = new BitString(value);
                    return true;
                case "dkm":
                    Dkm = new BitString(value);
                    return true;
                case "hashzz":
                    HashZ = new BitString(value);
                    return true;
                case "result":
                    TestPassed = value.StartsWith("p");
                    return true;
            }

            return false;
        }
    }
}