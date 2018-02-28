using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public string Direction { get; set; }
        public int KeyLen { get; set; }
        public int PtLen { get; set; }
        public string TweakMode { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject)source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            Direction = expandoSource.GetTypeFromProperty<string>("direction");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            KeyLen = expandoSource.GetTypeFromProperty<int>("keyLen");
            TweakMode = expandoSource.GetTypeFromProperty<string>("tweakMode");
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLen = int.Parse(value);
                    return true;

                case "dataunitlen":
                    PtLen = int.Parse(value);
                    return true;

                case "encrypt":
                    Direction = "encrypt";
                    return true;

                case "decrypt":
                    Direction = "decrypt";
                    return true;

                case "direction":
                    Direction = value;
                    return true;
            }
            return false;
        }
    }
}
