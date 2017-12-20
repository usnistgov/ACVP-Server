using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
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
        public BitString FixedPubExp { get; set; }
        public List<ITestCase> Tests { get; set; }
        public string TestType { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;

            Modulo = IntFromObject("modulo", source);
            InfoGeneratedByServer = SetBoolValue(source, "infoGeneratedByServer");
            Mode = RSAEnumHelpers.StringToKeyGenMode(source.randPQ);
            HashAlg = SHAEnumHelpers.StringToHashFunction(SetStringValue(source, "hashAlg"));
            PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(SetStringValue(source, "primeTest"));
            PubExp = RSAEnumHelpers.StringToPubExpMode(source.pubExp);

            if (PubExp == PubExpModes.FIXED)
            {
                FixedPubExp = new BitString(source.fixedPubExp);
            }

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
            return ($"{TestType}|{InfoGeneratedByServer}|{RSAEnumHelpers.KeyGenModeToString(Mode)}|" +
                    $"{Modulo}|{SHAEnumHelpers.HashFunctionToString(HashAlg)}|{RSAEnumHelpers.PrimeTestModeToString(PrimeTest)}|" +
                    $"{RSAEnumHelpers.PubExpModeToString(PubExp)}").GetHashCode();
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
                    case "primemethod":
                        Mode = RSAEnumHelpers.StringToKeyGenMode(value);
                        return true;
                    case "mod":
                        Modulo = int.Parse(value);
                        return true;
                    case "hash":
                        HashAlg = SHAEnumHelpers.StringToHashFunction(value);
                        return true;
                    case "table for m-t test":
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

        private int SetIntValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (int) source[label];
            }

            return 0;
        }

        private string SetStringValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (string) source[label];
            }

            return "";
        }

        private bool SetBoolValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (bool) source[label];
            }

            return default(bool);
        }

        private int IntFromObject(string sourcePropertyName, ExpandoObject source)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return 0;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return 0;
            }

            var valueAsBitString = sourcePropertyValue as long?;
            if (valueAsBitString != null)
            {
                return (int)valueAsBitString;
            }

            return (int)sourcePropertyValue;
        }
    }
}
