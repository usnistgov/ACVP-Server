using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCase : TestCaseBase
    {
        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public BigInteger StaticPrivateKeyServer { get; set; }
        public BigInteger StaticPublicKeyServerX { get; set; }
        public BigInteger StaticPublicKeyServerY { get; set; }
        public BigInteger EphemeralPrivateKeyServer { get; set; }
        public BigInteger EphemeralPublicKeyServerX { get; set; }
        public BigInteger EphemeralPublicKeyServerY { get; set; }

        public BigInteger StaticPrivateKeyIut { get; set; }
        public BigInteger StaticPublicKeyIutX { get; set; }
        public BigInteger StaticPublicKeyIutY { get; set; }
        public BigInteger EphemeralPrivateKeyIut { get; set; }
        public BigInteger EphemeralPublicKeyIutX { get; set; }
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
                    if (value.StartsWith("p"))
                    {
                        Result = "pass";
                    }
                    else
                    {
                        Result = "fail";
                        FailureTest = true;
                    }
                    return true;
            }

            return false;
        }

        protected void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;

            FailureTest = expandoSource.GetTypeFromProperty<bool>("failureTest");
            Deferred = expandoSource.GetTypeFromProperty<bool>("deferred");

            EphemeralPrivateKeyServer = expandoSource.GetBigIntegerFromProperty("ephemeralPrivateServer");
            EphemeralPublicKeyServerX = expandoSource.GetBigIntegerFromProperty("ephemeralPublicServerX");
            EphemeralPublicKeyServerY = expandoSource.GetBigIntegerFromProperty("ephemeralPublicServerY");
            StaticPrivateKeyServer = expandoSource.GetBigIntegerFromProperty("staticPrivateServer");
            StaticPublicKeyServerX = expandoSource.GetBigIntegerFromProperty("staticPublicServerX");
            StaticPublicKeyServerY = expandoSource.GetBigIntegerFromProperty("staticPublicServerY");
            EphemeralNonceServer = expandoSource.GetBitStringFromProperty("nonceEphemeralServer");

            EphemeralPrivateKeyIut = expandoSource.GetBigIntegerFromProperty("ephemeralPrivateIut");
            EphemeralPublicKeyIutX = expandoSource.GetBigIntegerFromProperty("ephemeralPublicIutX");
            EphemeralPublicKeyIutY = expandoSource.GetBigIntegerFromProperty("ephemeralPublicIutY");
            StaticPrivateKeyIut = expandoSource.GetBigIntegerFromProperty("staticPrivateIut");
            StaticPublicKeyIutX = expandoSource.GetBigIntegerFromProperty("staticPublicIutX");
            StaticPublicKeyIutY = expandoSource.GetBigIntegerFromProperty("staticPublicIutY");
            EphemeralNonceIut = expandoSource.GetBitStringFromProperty("nonceEphemeralIut");

            IdIutLen = expandoSource.GetTypeFromProperty<int>("idIutLen");
            IdIut = expandoSource.GetBitStringFromProperty("idIut");

            OiLen = expandoSource.GetTypeFromProperty<int>("oiLen");
            OtherInfo = expandoSource.GetBitStringFromProperty("oi");

            NonceNoKc = expandoSource.GetBitStringFromProperty("nonceNoKc");

            NonceAesCcm = expandoSource.GetBitStringFromProperty("nonceAesCcm");

            Z = expandoSource.GetBitStringFromProperty("z");

            Dkm = expandoSource.GetBitStringFromProperty("dkm");
            MacData = expandoSource.GetBitStringFromProperty("macData");

            HashZ = expandoSource.GetBitStringFromProperty("hashZIut");
            Tag = expandoSource.GetBitStringFromProperty("tagIut");

            Result = expandoSource.GetTypeFromProperty<string>("result");
        }
    }
}