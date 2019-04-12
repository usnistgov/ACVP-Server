using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestCase : TestCaseBase<TestGroup, TestCase, KasDsaAlgoAttributesFfc>
    {
        [JsonProperty(PropertyName = "staticPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPrivateKeyServer { get; set; }

        [JsonProperty(PropertyName = "staticPublicServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPublicKeyServer { get; set; }

        [JsonProperty(PropertyName = "ephemeralPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPrivateKeyServer { get; set; }

        [JsonProperty(PropertyName = "ephemeralPublicServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPublicKeyServer { get; set; }


        [JsonProperty(PropertyName = "staticPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPrivateKeyIut { get; set; }

        [JsonProperty(PropertyName = "staticPublicIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger StaticPublicKeyIut { get; set; }

        [JsonProperty(PropertyName = "ephemeralPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPrivateKeyIut { get; set; }

        [JsonProperty(PropertyName = "ephemeralPublicIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger EphemeralPublicKeyIut { get; set; }

        
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
                case "xstatcavs":
                    StaticPrivateKeyServer = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "ystatcavs":
                    StaticPublicKeyServer = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "xephemcavs":
                    EphemeralPrivateKeyServer = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "yephemcavs":
                    EphemeralPublicKeyServer = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "noncedkmcavs":
                    DkmNonceServer = new BitString(value);
                    break;
                case "nonceephemcavs":
                    EphemeralNonceServer = new BitString(value);
                    break;
                case "xstatiut":
                    StaticPrivateKeyIut = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "ystatiut":
                    StaticPublicKeyIut = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "xephemiut":
                    EphemeralPrivateKeyIut = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "yephemiut":
                    EphemeralPublicKeyIut = new BitString(value).ToPositiveBigInteger();
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