using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestCase : TestCaseBase<TestGroup, TestCase, KasDsaAlgoAttributesFfc>
    {
        [JsonIgnore] public FfcKeyPair StaticKeyServer = new FfcKeyPair();

        private int l => ParentGroup?.L ?? 0;
        private int n => ParentGroup?.N ?? 0;
        
        [JsonProperty(PropertyName = "staticPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyServer
        {
            get => StaticKeyServer.PrivateKeyX != 0 ? new BitString(StaticKeyServer.PrivateKeyX, n) : null;
            set => StaticKeyServer.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyServer
        {
            get => StaticKeyServer.PublicKeyY != 0 ? new BitString(StaticKeyServer.PublicKeyY, l) : null;
            set => StaticKeyServer.PublicKeyY = value.ToPositiveBigInteger();
        }

        
        [JsonIgnore] public FfcKeyPair EphemeralKeyServer = new FfcKeyPair();
        
        [JsonProperty(PropertyName = "ephemeralPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyServer
        {
            get => EphemeralKeyServer.PrivateKeyX != 0 ? new BitString(EphemeralKeyServer.PrivateKeyX, n) : null;
            set => EphemeralKeyServer.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyServer
        {
            get => EphemeralKeyServer.PublicKeyY != 0 ? new BitString(EphemeralKeyServer.PublicKeyY, l) : null;
            set => EphemeralKeyServer.PublicKeyY = value.ToPositiveBigInteger();
        }

        
        [JsonIgnore] public FfcKeyPair StaticKeyIut = new FfcKeyPair();

        [JsonProperty(PropertyName = "staticPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyIut
        {
            get => StaticKeyIut.PrivateKeyX != 0 ? new BitString(StaticKeyIut.PrivateKeyX, n) : null;
            set => StaticKeyIut.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyIut
        {
            get => StaticKeyIut.PublicKeyY != 0 ? new BitString(StaticKeyIut.PublicKeyY, l) : null;
            set => StaticKeyIut.PublicKeyY = value.ToPositiveBigInteger();
        }

        
        [JsonIgnore] public FfcKeyPair EphemeralKeyIut = new FfcKeyPair();
        
        [JsonProperty(PropertyName = "ephemeralPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyIut
        {
            get => EphemeralKeyIut.PrivateKeyX != 0 ? new BitString(EphemeralKeyIut.PrivateKeyX, n) : null;
            set => EphemeralKeyIut.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyIut
        {
            get => EphemeralKeyIut.PublicKeyY != 0 ? new BitString(EphemeralKeyIut.PublicKeyY, l) : null;
            set => EphemeralKeyIut.PublicKeyY = value.ToPositiveBigInteger();
        }

        
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
                    StaticPrivateKeyServer = new BitString(value);
                    return true;
                case "ystatcavs":
                    StaticPublicKeyServer = new BitString(value);
                    return true;
                case "xephemcavs":
                    EphemeralPrivateKeyServer = new BitString(value);
                    return true;
                case "yephemcavs":
                    EphemeralPublicKeyServer = new BitString(value);
                    return true;
                case "noncedkmcavs":
                    DkmNonceServer = new BitString(value);
                    break;
                case "nonceephemcavs":
                    EphemeralNonceServer = new BitString(value);
                    break;
                case "xstatiut":
                    StaticPrivateKeyIut = new BitString(value);
                    return true;
                case "ystatiut":
                    StaticPublicKeyIut = new BitString(value);
                    return true;
                case "xephemiut":
                    EphemeralPrivateKeyIut = new BitString(value);
                    return true;
                case "yephemiut":
                    EphemeralPublicKeyIut = new BitString(value);
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