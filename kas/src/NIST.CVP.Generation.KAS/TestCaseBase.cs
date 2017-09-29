using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
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

        public BigInteger EphemeralPrivateKeyServer { get; set; }
        public BigInteger EphemeralPublicKeyServer { get; set; }
        public BigInteger StaticPrivateKeyServer { get; set; }
        public BigInteger StaticPublicKeyServer { get; set; }

        public BigInteger EphemeralPrivateKeyIut { get; set; }
        public BigInteger EphemeralPublicKeyIut { get; set; }
        public BigInteger StaticPrivateKeyIut { get; set; }
        public BigInteger StaticPublicKeyIut { get; set; }
        
        public int IdIutLen { get; set; }
        public BitString IdIut { get; set; }

        public int OiLen { get; set; }
        public BitString Oi { get; set; }
        
        public BitString NonceNoKc { get; set; }

        public BitString NonceAesCcm { get; set; }

        public BitString Z { get; set; }
        public BitString Dkm { get; set; }
        public BitString MacData { get; set; }

        public BitString HashZIut { get; set; }
        public BitString TagIut { get; set; }
        public string Result { get; set; }

        public bool Merge(ITestCase promptTestCase)
        {
            if (TestCaseId == promptTestCase.TestCaseId)
            {
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

            EphemeralPrivateKeyServer = expandoSource.GetBigIntegerFromProperty("xEphemeralServer");
            EphemeralPublicKeyServer = expandoSource.GetBigIntegerFromProperty("yEphemeralServer");
            StaticPrivateKeyServer = expandoSource.GetBigIntegerFromProperty("xStaticServer");
            StaticPublicKeyServer = expandoSource.GetBigIntegerFromProperty("yStaticServer");

            EphemeralPrivateKeyIut = expandoSource.GetBigIntegerFromProperty("xEphemeralIut");
            EphemeralPublicKeyIut = expandoSource.GetBigIntegerFromProperty("yEphemeralIut");
            StaticPrivateKeyIut = expandoSource.GetBigIntegerFromProperty("xStaticIut");
            StaticPublicKeyIut = expandoSource.GetBigIntegerFromProperty("yStaticIut");

            IdIutLen = expandoSource.GetTypeFromProperty<int>("idIutLen");
            IdIut = expandoSource.GetBitStringFromProperty("idIut");

            
            OiLen = expandoSource.GetTypeFromProperty<int>("oiLen");
            Oi = expandoSource.GetBitStringFromProperty("oi");

            NonceNoKc = expandoSource.GetBitStringFromProperty("nonceNoKc");

            NonceAesCcm = expandoSource.GetBitStringFromProperty("nonceAesCcm");

            Z = expandoSource.GetBitStringFromProperty("z");
            HashZIut = expandoSource.GetBitStringFromProperty("hashZIut");
            TagIut = expandoSource.GetBitStringFromProperty("tagIut");
        }
    }
}
