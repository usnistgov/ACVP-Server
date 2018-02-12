using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestGroup : ITestGroup
    {
        public int Modulo { get; set; } = 2048;
        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>())
        {
        }

        public TestGroup(dynamic source)
        {
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
            return $"{Modulo}".GetHashCode();
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
