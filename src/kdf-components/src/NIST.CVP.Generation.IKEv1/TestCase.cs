using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv1
{
    public class TestCase : ITestCase
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

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString CkyInit { get; set; }
        public BitString CkyResp { get; set; }
        public BitString NInit { get; set; }
        public BitString NResp { get; set; }
        public BitString Gxy { get; set; }
        public BitString PreSharedKey { get; set; }
        public BitString SKeyId { get; set; }
        public BitString SKeyIdD { get; set; }
        public BitString SKeyIdA { get; set; }
        public BitString SKeyIdE { get; set; }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject) source;
            CkyInit = expandoSource.GetBitStringFromProperty("ckyInit");
            CkyResp = expandoSource.GetBitStringFromProperty("ckyResp");
            NInit = expandoSource.GetBitStringFromProperty("nInit");
            NResp = expandoSource.GetBitStringFromProperty("nResp");
            Gxy = expandoSource.GetBitStringFromProperty("gxy");
            PreSharedKey = expandoSource.GetBitStringFromProperty("preSharedKey");
            SKeyId = expandoSource.GetBitStringFromProperty("sKeyId");
            SKeyIdD = expandoSource.GetBitStringFromProperty("sKeyIdD");
            SKeyIdA = expandoSource.GetBitStringFromProperty("sKeyIdA");
            SKeyIdE = expandoSource.GetBitStringFromProperty("sKeyIdE");
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "ninit":
                case "ni":
                    NInit = new BitString(value);
                    return true;

                case "nresp":
                case "nr":
                    NResp = new BitString(value);
                    return true;

                case "cky_i":
                    CkyInit = new BitString(value);
                    return true;
                
                case "cky_r":
                    CkyResp = new BitString(value);
                    return true;

                case "g^xy":
                    Gxy = new BitString(value);
                    return true;
                
                case "pre-shared-key":
                    PreSharedKey = new BitString(value);
                    return true;

                case "skeyid":
                    SKeyId = new BitString(value);
                    return true;

                case "skeyid_a":
                    SKeyIdA = new BitString(value);
                    return true;

                case "skeyid_d":
                    SKeyIdD = new BitString(value);
                    return true;

                case "skeyid_e":
                    SKeyIdE = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
