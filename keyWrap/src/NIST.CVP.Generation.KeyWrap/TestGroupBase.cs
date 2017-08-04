using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public abstract class TestGroupBase<TTestCase> : ITestGroup
        where TTestCase : TestCaseBase, new()
    {
       
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";

        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }

        [JsonProperty(PropertyName = "kwCipher")]
        public string KwCipher { get; set; }

        [JsonProperty(PropertyName = "ptLen")]
        public int PtLen { get; set; }

        public abstract KeyWrapType KeyWrapType { get; set; }


        [JsonProperty(PropertyName = "keyLen")]
        public abstract int KeyLength { get; set; }

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

        public TestGroupBase()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroupBase(dynamic source)
        {
            LoadSource(source);
        }

        protected void LoadSource(dynamic source)
        {
            TestType = source.testType;
            Direction = source.direction;
            KwCipher = source.kwCipher;
            KeyLength = source.keyLen;
            PtLen = source.ptLen;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                //not pretty, but "Tests.Add(new TTestCase(test));" isn't allowed
                Tests.Add((TTestCase)Activator.CreateInstance(typeof(TTestCase), test));
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

        public abstract override int GetHashCode();
        public abstract override bool Equals(object obj);
        public abstract bool SetString(string name, string value);

    }
}
