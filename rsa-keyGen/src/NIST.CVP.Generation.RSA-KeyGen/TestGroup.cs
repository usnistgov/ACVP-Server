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
using NIST.CVP.Math;

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
        public BitString FixedPubExp { get; set; } = null;
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
            Mode = RSAEnumHelpers.StringToKeyGenMode(source.randPQ);
            Modulo = source.modulo;
            HashAlg = SHAEnumHelpers.StringToHashFunction(SetStringValue(source, "hashAlg"));
            PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(SetStringValue(source, "primeTest"));
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

        public override int GetHashCode()
        {
            return $"{TestType}|{InfoGeneratedByServer}|{Mode}|{Modulo}|{HashAlg}|{PrimeTest}|{PubExp}".GetHashCode();
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

            try
            {
                switch (name.ToLower())
                {
                    case "testtype":
                        TestType = value;
                        return true;
                    case "mode":
                        Mode = RSAEnumHelpers.StringToKeyGenMode(value);
                        return true;
                    case "hashalg":
                        HashAlg = SHAEnumHelpers.StringToHashFunction(value);
                        return true;
                    case "pubexp":
                        PubExp = RSAEnumHelpers.StringToPubExpMode(value);
                        return true;
                    case "primetest":
                        PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(value);
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private string SetStringValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (string) source[label];
            }

            return "";
        }
    }
}
