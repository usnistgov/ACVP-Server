using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroup : ITestGroup
    {
        public SigGenModes Mode { get; set; }
        public int Modulo { get; set; }
        public HashFunction HashAlg { get; set; }
        public SaltModes SaltMode { get; set; }
        public BitString Salt { get; set; }
        public int SaltLen { get; set; }
        public KeyPair Key { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            Mode = RSAEnumHelpers.StringToSigGenMode(source.sigType);
            Modulo = IntFromObject("modulo", source);
            HashAlg = SHAEnumHelpers.StringToHashFunction(SetStringValue(source, "hashAlg"));

            if(Mode == SigGenModes.PSS)
            {
                SaltMode = RSAEnumHelpers.StringToSaltMode(source.saltMode);
                SaltLen = IntFromObject("saltLen", source);

                if (SaltMode == SaltModes.FIXED)
                {
                    Salt = BitStringFromObject("salt", source);
                }
            }

            Key = KeyFromObject(source);

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
            return ($"{TestType}|{RSAEnumHelpers.SigGenModeToString(Mode)}|" +
                    $"{Modulo}|{SHAEnumHelpers.HashFunctionToString(HashAlg)}").GetHashCode();
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

        private KeyPair KeyFromObject(ExpandoObject source)
        {
            var e = BigIntegerFromObject("e", source);
            var n = BigIntegerFromObject("n", source);
            var d = BigIntegerFromObject("d", source);
            var dmp1 = BigIntegerFromObject("dmp1", source);
            var dmq1 = BigIntegerFromObject("dmq1", source);
            var iqmp = BigIntegerFromObject("iqmp", source);

            if(d == null)
            {
                return new KeyPair
                {
                    PrivKey = new PrivateKey { DMP1 = dmp1, DMQ1 = dmq1, IQMP = iqmp },
                    PubKey = new PublicKey { E = e, N = n }
                };
            }
            else
            {
                return new KeyPair
                {
                    PrivKey = new PrivateKey { D = d },
                    PubKey = new PublicKey { E = e, N = n }
                };
            }
        }

        private BitString BitStringFromObject(string sourcePropertyName, ExpandoObject source, int length = -1)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return null;
            }

            var valueAsBitString = sourcePropertyValue as BitString;
            if (valueAsBitString != null)
            {
                return valueAsBitString;
            }

            return new BitString(sourcePropertyValue.ToString(), length);
        }

        private BigInteger BigIntegerFromObject(string sourcePropertyName, ExpandoObject source)
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

            if (sourcePropertyValue.GetType() == typeof(string))
            {
                return new BitString(sourcePropertyValue.ToString()).ToPositiveBigInteger();
            }

            var valueAsBigInteger = (BigInteger)sourcePropertyValue;
            if (valueAsBigInteger != 0)
            {
                return valueAsBigInteger;
            }

            return 0;
        }

        private string SetStringValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (string)source[label];
            }

            return "";
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

            var valueAsLong = sourcePropertyValue as long?;
            if (valueAsLong != null)
            {
                return (int)valueAsLong;
            }

            return (int)sourcePropertyValue;
        }
    }
}
