using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestGroup : ITestGroup
    {
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

            TotalTestCases = expandoSource.GetTypeFromProperty<int>("totalTests");
            TotalFailingCases = expandoSource.GetTypeFromProperty<int>("totalFailingTests");

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
            return $"{Modulo}|{TotalFailingCases}|{TotalTestCases}".GetHashCode();
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
    }
}
