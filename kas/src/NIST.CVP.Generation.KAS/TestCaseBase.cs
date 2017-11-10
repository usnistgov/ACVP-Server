using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestCaseBase : ITestCase
    {
        protected TestCaseBase()
        {

        }

        protected TestCaseBase(dynamic source)
        {
            MapToProperties(source);
        }

        protected TestCaseBase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public TestCaseDispositionOption TestCaseDisposition { get; set; }

        public BigInteger StaticPrivateKeyServer { get; set; }
        public BigInteger StaticPublicKeyServer { get; set; }
        public BigInteger EphemeralPrivateKeyServer { get; set; }
        public BigInteger EphemeralPublicKeyServer { get; set; }
        public BitString EphemeralNonceServer { get; set; }

        public BigInteger StaticPrivateKeyIut { get; set; }
        public BigInteger StaticPublicKeyIut { get; set; }
        public BigInteger EphemeralPrivateKeyIut { get; set; }
        public BigInteger EphemeralPublicKeyIut { get; set; }
        public BitString EphemeralNonceIut { get; set; }
        
        
        public int IdIutLen { get; set; }
        public BitString IdIut { get; set; }

        public int OiLen { get; set; }
        public BitString OtherInfo { get; set; }
        
        public BitString NonceNoKc { get; set; }

        public BitString NonceAesCcm { get; set; }

        public BitString Z { get; set; }
        public BitString Dkm { get; set; }
        public BitString MacData { get; set; }

        public BitString HashZ { get; set; }
        public BitString Tag { get; set; }
        public string Result { get; set; }

        public bool Merge(ITestCase promptTestCase)
        {
            if (TestCaseId == promptTestCase.TestCaseId)
            {
                return true;
            }

            return false;
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
            EphemeralPublicKeyServer = expandoSource.GetBigIntegerFromProperty("ephemeralPublicServer");
            StaticPrivateKeyServer = expandoSource.GetBigIntegerFromProperty("staticPrivateServer");
            StaticPublicKeyServer = expandoSource.GetBigIntegerFromProperty("staticPublicServer");
            EphemeralNonceServer = expandoSource.GetBitStringFromProperty("nonceEphemeralServer");

            EphemeralPrivateKeyIut = expandoSource.GetBigIntegerFromProperty("ephemeralPrivateIut");
            EphemeralPublicKeyIut = expandoSource.GetBigIntegerFromProperty("ephemeralPublicIut");
            StaticPrivateKeyIut = expandoSource.GetBigIntegerFromProperty("staticPrivateIut");
            StaticPublicKeyIut = expandoSource.GetBigIntegerFromProperty("staticPublicIut");
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
