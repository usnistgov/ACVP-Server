using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            MessageLength = source.msgLen;
            DigestLength = source.digLen;
            BitOriented = source.bitOriented;
            IncludeNull = source.includeNull;

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        // Not used
        public int KeyLength { get; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "MMT";
        [JsonProperty(PropertyName = "msglen")]
        public int MessageLength { get; set; }
        [JsonProperty(PropertyName = "diglen")]
        public int DigestLength { get; set; }
        [JsonProperty(PropertyName = "bitOriented")]
        public bool BitOriented { get; set; } = false;
        [JsonProperty(PropertyName = "includeNull")]
        public bool IncludeNull { get; set; } = false;

        public List<ITestCase> Tests { get; set; }

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
            return $"{TestType}|{MessageLength}|{DigestLength}|{BitOriented}|{IncludeNull}".GetHashCode();
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

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "msglen":
                    MessageLength = intVal;
                    return true;
                case "diglen":
                    DigestLength = intVal;
                    return true;
            }

            return false;
        }
    }
}
