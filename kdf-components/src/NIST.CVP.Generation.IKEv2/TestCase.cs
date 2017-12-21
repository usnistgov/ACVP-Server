using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool Deferred { get; set; }
        public bool FailureTest { get; set; }

        public BitString NInit { get; set; }
        public BitString NResp { get; set; }
        public BitString Gir { get; set; }
        public BitString GirNew { get; set; }
        public BitString SpiInit { get; set; }
        public BitString SpiResp { get; set; }
        public BitString SKeySeed { get; set; }
        public BitString DerivedKeyingMaterial { get; set; }
        public BitString DerivedKeyingMaterialChild { get; set; }
        public BitString DerivedKeyingMaterialDh { get; set; }
        public BitString SKeySeedReKey { get; set; }

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
            
            NInit = expandoSource.GetBitStringFromProperty("nInit");
            NResp = expandoSource.GetBitStringFromProperty("nResp");
            Gir = expandoSource.GetBitStringFromProperty("gir");
            GirNew = expandoSource.GetBitStringFromProperty("girNew");
            SpiInit = expandoSource.GetBitStringFromProperty("spiInit");
            SpiResp = expandoSource.GetBitStringFromProperty("spiResp");
            
            SKeySeed = expandoSource.GetBitStringFromProperty("sKeySeed");
            DerivedKeyingMaterial = expandoSource.GetBitStringFromProperty("dkm");
            DerivedKeyingMaterialChild = expandoSource.GetBitStringFromProperty("dkmChild");
            DerivedKeyingMaterialDh = expandoSource.GetBitStringFromProperty("dkmChildDh");
            SKeySeedReKey = expandoSource.GetBitStringFromProperty("sKeySeedReKey");
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
                case "ninit":
                case "ni":
                    NInit = new BitString(value);
                    return true;

                case "nresp":
                case "nr":
                    NResp = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
