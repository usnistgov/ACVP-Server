using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.CMAC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestGroup : ITestGroup
    {
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }
        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }
        public List<ITestCase> Tests { get; set; }

        [JsonIgnore]
        public CmacTypes CmacType { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            Function = source.direction;
            KeyLength = source.keyLen;
            MessageLength = source.msgLen;
            MacLength = source.macLen;
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
                $"{Function}|{TestType}|{KeyLength}|{MessageLength}|{MacLength}"
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

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    KeyLength = intVal;
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal;
                    return true;
            }
            return false;
        }
    }
}
