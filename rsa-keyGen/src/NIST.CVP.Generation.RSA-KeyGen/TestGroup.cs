using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroup : ITestGroup
    {
        public bool InfoGeneratedByServer { get; set; }
        public KeyGenModes Mode { get; set; }
        public int Modulo { get; set; }
        public HashFunction HashAlg { get; set; }
        public PrimeTestModes PrimeTest { get; set; }
        public PubExpModes PubExp { get; set; }
        public List<ITestCase> Tests { get; set; }
        public string TestType { get; set; }

        public int KeyLength { get { return 0; } } 

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;

            InfoGeneratedByServer = source.infoGeneratedByServer;
            Mode = RSAEnumHelpers.StringToKeyGenMode(source.mode);
            Modulo = source.modulo;
            HashAlg = SHAEnumHelpers.StringToHashFunction(source.hashAlg);
            PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(source.primeTest);
            PubExp = RSAEnumHelpers.StringToPubExpMode(source.pubExp);

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
    }
}
