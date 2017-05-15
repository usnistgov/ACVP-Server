using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestGroup : ITestGroup
    {
        
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "kwCipher")]
        public string KwCipher { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "ptLen")]
        public int PtLen { get; set; }

        public KeyWrapType KeyWrapType { get; set; }

        public bool UseInverseCipher
        {
            get
            {
                if (KwCipher.Equals("inverse", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            Direction = source.direction;
            KwCipher = source.kwCipher;
            KeyLength = source.keyLen;
            PtLen = source.ptLen;
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
                $"{KeyWrapType}|{TestType}|{Direction}|{KwCipher}|{PtLen}"
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

            name = name.ToLower();

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name)
            {
                case "keylen":
                case "keylength":
                    KeyLength = intVal;
                    return true;
                case "ptlen":
                    PtLen = intVal;
                    return true;
            }
            return false;
        }

    }
}
