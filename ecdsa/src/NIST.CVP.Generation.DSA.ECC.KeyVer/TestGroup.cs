using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public EccDomainParameters DomainParameters { get; set; }

        // Used internally to build test cases with particular error cases
        public ITestCaseExpectationProvider<TestCaseExpectationEnum> TestCaseExpectationProvider { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            var curveName = expandoSource.GetTypeFromProperty<string>("curve");

            var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName, false);
            var curveFactory = new EccCurveFactory();

            DomainParameters = new EccDomainParameters(curveFactory.GetCurve(curve));

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
                case "curve":
                    var factory = new EccCurveFactory();
                    var curve = factory.GetCurve(EnumHelpers.GetEnumFromEnumDescription<Curve>(value));
                    DomainParameters = new EccDomainParameters(curve);
                    return true;
            }

            return false;
        }
    }
}
