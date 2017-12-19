using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString KeyIn { get; set; }
        public BitString FixedData { get; set; }
        public BitString IV { get; set; }
        public int BreakLocation { get; set; }
        public BitString KeyOut { get; set; }

        // Only used in FireHoseTests
        public int L;

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
            KeyIn = expandoSource.GetBitStringFromProperty("keyIn");
            KeyOut = expandoSource.GetBitStringFromProperty("keyOut");
            FixedData = expandoSource.GetBitStringFromProperty("fixedData");
            IV = expandoSource.GetBitStringFromProperty("iv");
            BreakLocation = expandoSource.GetTypeFromProperty<int>("breakLocation");
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            // Nothing to merge
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
                case "keyin":
                case "ki":
                    KeyIn = new BitString(value);
                    return true;

                case "keyout":
                case "ko":
                    KeyOut = new BitString(value);
                    return true;

                case "iv":
                    IV = new BitString(value);
                    return true;

                case "fixedinputdata":
                case "databeforectrdata":
                    FixedData = new BitString(value);
                    return true;

                case "dataafterctrdata":
                    FixedData = FixedData.ConcatenateBits(new BitString(value));
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "fixedinputdatabytelen":
                case "databeforectrlen":
                    BreakLocation = int.Parse(value) * 8;
                    return true;
            }

            return false;
        }
    }
}
