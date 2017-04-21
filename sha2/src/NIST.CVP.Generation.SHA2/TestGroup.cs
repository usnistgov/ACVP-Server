using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestGroup : ITestGroup
    {
        [JsonProperty(PropertyName = "function")]
        public ModeValues Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public DigestSizes DigestSize { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "bitOriented")]
        public bool BitOriented { get; set; }

        [JsonProperty(PropertyName = "includeNull")]
        public bool IncludeNull { get; set; }

        public List<ITestCase> Tests { get; set; }
        public int KeyLength { get { return 0; } }              // Not relevant

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            Function = SHAEnumHelpers.StringToMode(source.function);
            DigestSize = SHAEnumHelpers.StringToDigest(source.digestSize);

            BitOriented = SetBoolValue(source, "bitOriented");
            IncludeNull = SetBoolValue(source, "includeNull");

            Tests = new List<ITestCase>();
            foreach(var test in source.tests)
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
            return
                $"{Function}|{DigestSize}|{TestType}"
                    .GetHashCode();
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
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            name = name.ToLower();

            try
            {
                if (name == "testtype")
                {
                    TestType = value;
                    return true;
                }

                if (name == "function")
                {
                    Function = SHAEnumHelpers.StringToMode(value);
                    return true;
                }

                if (name == "digestsize")
                {
                    DigestSize = SHAEnumHelpers.StringToDigest(value);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        private bool SetBoolValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (bool)source[label];
            }

            return default(bool);
        }
    }
}
