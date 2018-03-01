using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public int Modulo { get; set; }
        public int TotalTestCases { get; set; }
        public int TotalFailingCases { get; set; }
        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            Tests = new List<ITestCase>();

            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TotalTestCases = expandoSource.GetTypeFromProperty<int>("totalTests");
            TotalFailingCases = expandoSource.GetTypeFromProperty<int>("totalFailingTests");

            foreach (var test in source.tests)
            {
                var tc = new TestCase(test)
                {
                    Parent = this
                };
                Tests.Add(tc);
            }
        }
    }
}
