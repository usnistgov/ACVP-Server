using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroup : ITestGroup
    {
        public string Direction { get; set; }
        public int NumberOfKeys { get; set; }

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
            var expandoSource = (ExpandoObject)source;

            Direction = expandoSource.GetTypeFromProperty<string>("direction");

            var keyingOption = expandoSource.GetTypeFromProperty<int>("keyingOption");
            NumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption);

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
                $"{Direction}|{NumberOfKeys}|{TestType}|{OverflowCounter}|{IncrementalCounter}"
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
                case "testtype":
                    TestType = value;
                    return true;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "numberofkeys":
                    NumberOfKeys = intVal;
                    return true;
            }
            return false;
        }
    }
}
