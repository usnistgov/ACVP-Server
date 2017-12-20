using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestGroup: ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source):this(source.ToObject<ExpandoObject>())
        {
          
        }

        public TestGroup(dynamic source)
        {
            if (((ExpandoObject)source).ContainsProperty("numberOfKeys"))
            {
                NumberOfKeys = (int)source.numberOfKeys;
            }
           
            TestType = source.testType;
            Function = source.direction;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }

        }

        public int KeyLength
        {
            get { return 64; }
        }

        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType{ get; set; }
        [JsonProperty(PropertyName = "numberOfKeys")]
        public int NumberOfKeys { get; set; }
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
            return
                $"{Function}|{TestType}|{NumberOfKeys}"
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
                case "numberofkeys":
                    NumberOfKeys = intVal;
                    return true;
            }
            return false;
        }

        
    }
}
