using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestGroup : ITestGroup
    {
        public int L { get; set; }
        public int N { get; set; }
        public FfcDomainParameters DomainParams { get; set; }       // Mainly for private use to make sure all test cases have same value
        public HashFunction HashAlg { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        // Needed for SetString, FireHoseTests
        private BigInteger p;
        private BigInteger q;
        private BigInteger g;

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            L = (int)source.l;
            N = (int)source.n;

            if (((ExpandoObject)source).ContainsProperty("hashAlg"))
            {
                var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm((string)source.hashAlg);
                HashAlg = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);
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
            return ($"{L}{N}").GetHashCode();
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
                case "p":
                    p = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "g":
                    g = new BitString(value).ToPositiveBigInteger();
                    DomainParams = new FfcDomainParameters(p, q, g);
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
