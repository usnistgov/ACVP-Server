using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroup : ITestGroup
    {
        public string Direction { get; set; }
        public int KeyLength { get; set; }

        // Properties for specific groups
        public MathDomain DataLength { get; set; }
        public bool StaticGroupOfTests { get; set; }

        // This is a vectorset / IUT property but it needs to be defined somewhere other than Parameter.cs
        public bool IncrementalCounter { get; set; }
        public bool OverflowCounter { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            Direction = expandoSource.GetTypeFromProperty<string>("direction");
            KeyLength = expandoSource.GetTypeFromProperty<int>("keyLen");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            OverflowCounter = expandoSource.GetTypeFromProperty<bool>("overflow");
            
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
            return
                $"{Direction}|{KeyLength}|{TestType}|{OverflowCounter}|{IncrementalCounter}"
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
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = Int32.Parse(value);
                    return true;

                case "direction":
                    Direction = value;
                    return true;
            }
            return false;
        }
    }
}
