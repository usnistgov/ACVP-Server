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

        private void MapToProperties(dynamic source)
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
            DerivedKeyingMaterial = expandoSource.GetBitStringFromProperty("derivedKeyingMaterial");
            DerivedKeyingMaterialChild = expandoSource.GetBitStringFromProperty("derivedKeyingMaterialChild");
            DerivedKeyingMaterialDh = expandoSource.GetBitStringFromProperty("derivedKeyingMaterialDh");
            SKeySeedReKey = expandoSource.GetBitStringFromProperty("sKeySeedReKey");
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

                case "g^ir":
                    Gir = new BitString(value);
                    return true;

                case "g^ir (new)":
                    GirNew = new BitString(value);
                    return true;

                case "spii":
                    SpiInit = new BitString(value);
                    return true;

                case "spir":
                    SpiResp = new BitString(value);
                    return true;

                case "skeyseed":
                    SKeySeed = new BitString(value);
                    return true;

                case "dkm":
                    DerivedKeyingMaterial = new BitString(value);
                    return true;

                case "dkm(child sa)":
                    DerivedKeyingMaterialChild = new BitString(value);
                    return true;

                case "dkm(child sa d-h)":
                    DerivedKeyingMaterialDh = new BitString(value);
                    return true;

                case "skeyseed(rekey)":
                    SKeySeedReKey = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
