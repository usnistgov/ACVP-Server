using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

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
            Function = source.function;
            DigestSize = source.digestSize;
            BitOriented = source.bitOriented;
            IncludeNull = source.includeNull;

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
                $"{Function}|{DigestSize}|{TestType}|{BitOriented}|{IncludeNull}"
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

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
            }

            if(name == "function")
            {
                switch (value.ToLower())
                {
                    case "sha1":
                        Function = ModeValues.SHA1;
                        return true;
                    case "sha2":
                        Function = ModeValues.SHA2;
                        return true;
                }
            }

            if(name == "digestsize")
            {
                switch (value.ToLower())
                {
                    case "160":
                        DigestSize = DigestSizes.d160;
                        Function = ModeValues.SHA1;
                        return true;
                    case "224":
                        DigestSize = DigestSizes.d224;
                        Function = ModeValues.SHA2;
                        return true;
                    case "256":
                        DigestSize = DigestSizes.d256;
                        Function = ModeValues.SHA2;
                        return true;
                    case "384":
                        DigestSize = DigestSizes.d384;
                        Function = ModeValues.SHA2;
                        return true;
                    case "512":
                        DigestSize = DigestSizes.d512;
                        Function = ModeValues.SHA2;
                        return true;
                    case "512t224":
                        DigestSize = DigestSizes.d512t224;
                        Function = ModeValues.SHA2;
                        return true;
                    case "512t256":
                        DigestSize = DigestSizes.d512t256;
                        Function = ModeValues.SHA2;
                        return true;
                }
            }

            return false;
        }
    }
}
