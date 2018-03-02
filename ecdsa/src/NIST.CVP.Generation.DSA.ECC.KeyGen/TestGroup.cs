using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public Curve Curve { get; set; }
        public SecretGenerationMode SecretGenerationMode { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(expandoSource.GetTypeFromProperty<string>("curve"), false);
            SecretGenerationMode = EnumHelpers.GetEnumFromEnumDescription<SecretGenerationMode>(expandoSource.GetTypeFromProperty<string>("secretGenerationMode"), false);
            
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                var tc = new TestCase(test)
                {
                    Parent = this
                };
                Tests.Add(tc);
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
                case "curve":
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(value);
                    return true;
            }

            return false;
        }
    }
}
