using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
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
            ParseDomainParameters((ExpandoObject)source);

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            foreach (var test in Tests)
            {
                var matchingTest = testsToMerge.FirstOrDefault(t => t.TestCaseId == test.TestCaseId);
                if (matchingTest == null)
                {
                    return false;
                }
                if (!test.Merge(matchingTest))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return ($"{EnumHelpers.GetEnumDescriptionFromEnum(DomainParameters.CurveE.CurveName)}").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherGroup = obj as TestGroup;
            if (otherGroup == null)
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
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

        private void ParseDomainParameters(ExpandoObject source)
        {
            var curveName = "";

            if (source.ContainsProperty("curve"))
            {
                curveName = source.GetTypeFromProperty<string>("curve");
            }

            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName, false));

            DomainParameters = new EccDomainParameters(curve);
        }
    }
}
