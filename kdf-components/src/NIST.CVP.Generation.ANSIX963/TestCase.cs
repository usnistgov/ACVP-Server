using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.ANSIX963
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString Z { get; set; }
        public BitString SharedInfo { get; set; }
        public BitString KeyData { get; set; }

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
            Z = expandoSource.GetBitStringFromProperty("z");
            SharedInfo = expandoSource.GetBitStringFromProperty("sharedInfo");
            KeyData = expandoSource.GetBitStringFromProperty("keyData");
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
                case "z":
                    Z = new BitString(value);
                    return true;

                case "sharedinfo":
                    SharedInfo = new BitString(value);
                    return true;

                case "key_data":
                    KeyData = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
