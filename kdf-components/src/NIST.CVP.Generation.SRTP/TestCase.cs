using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SRTP
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool Deferred { get; set; }
        public bool FailureTest { get; set; }

        public BitString MasterKey { get; set; }
        public BitString MasterSalt { get; set; }
        public BitString Index { get; set; }
        public BitString SrtcpIndex { get; set; }
        public BitString SrtpKe { get; set; }
        public BitString SrtpKa { get;set; }
        public BitString SrtpKs { get; set; }
        public BitString SrtcpKe { get; set; }
        public BitString SrtcpKa { get; set; }
        public BitString SrtcpKs { get; set; }

        // For FireHoseTests
        public BitString Kdr { get; private set; }

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

        public void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject) source;

            MasterKey = expandoSource.GetBitStringFromProperty("masterKey");
            MasterSalt = expandoSource.GetBitStringFromProperty("masterSalt");
            Index = expandoSource.GetBitStringFromProperty("index");
            SrtcpIndex = expandoSource.GetBitStringFromProperty("srtcpIndex");

            SrtpKe = expandoSource.GetBitStringFromProperty("srtpKe");
            SrtpKa = expandoSource.GetBitStringFromProperty("srtpKa");
            SrtpKs = expandoSource.GetBitStringFromProperty("srtpKs");
            SrtcpKe = expandoSource.GetBitStringFromProperty("srtcpKe");
            SrtcpKa = expandoSource.GetBitStringFromProperty("srtcpKa");
            SrtcpKs = expandoSource.GetBitStringFromProperty("srtcpKs");
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            // Nothing to merge, put everything in answer.json
            return true;
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "k_master":
                    MasterKey = new BitString(value);
                    return true;

                case "master_salt":
                    MasterSalt = new BitString(value);
                    return true;

                case "kdr":
                    Kdr = new BitString(value);
                    return true;

                case "index":
                    Index = new BitString(value);
                    return true;

                case "index (srtcp)":
                    SrtcpIndex = new BitString(value);
                    return true;

                case "srtp k_e":
                    SrtpKe = new BitString(value);
                    return true;

                case "srtp k_a":
                    SrtpKa = new BitString(value);
                    return true;

                case "srtp k_s":
                    SrtpKs = new BitString(value);
                    return true;

                case "srtcp k_e":
                    SrtcpKe = new BitString(value);
                    return true;

                case "srtcp k_a":
                    SrtcpKa = new BitString(value);
                    return true;

                case "srtcp k_s":
                    SrtcpKs = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
