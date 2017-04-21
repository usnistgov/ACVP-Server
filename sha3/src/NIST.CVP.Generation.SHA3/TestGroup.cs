using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestGroup : ITestGroup
    {
        public int KeyLength { get { return 0; } }              // Not relevant
        public List<ITestCase> Tests { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "function")]
        public string Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "outBit")]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; } = false;

        [JsonProperty(PropertyName = "outLengthMin")]
        public int MinOutputLength { get; set; } = 0;

        [JsonProperty(PropertyName = "outLengthMax")]
        public int MaxOutputLength { get; set; } = 0;

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            Function = source.function;
            DigestSize = (int)source.digestSize;

            BitOrientedInput = SetBoolValue(source, "inBit");
            BitOrientedOutput = SetBoolValue(source, "outBit");
            IncludeNull = SetBoolValue(source, "inEmpty");
            MinOutputLength = SetIntValue(source, "outLengthMin");
            MaxOutputLength = SetIntValue(source, "outLengthMax");
            
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
            //return
            //    $"{Function}|{DigestSize}|{TestType}|{BitOrientedInput}|{IncludeNull}|{BitOrientedOutput}|{MinOutputLength}|{MaxOutputLength}"
            //        .GetHashCode();

            return $"{Function}|{DigestSize}|{TestType}".GetHashCode();
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

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
                case "function":
                    Function = value;
                    return true;
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

        private int SetIntValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return Convert.ToInt32(source[label]);
            }

            return 0;
        }
    }
}
