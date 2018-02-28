using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public FfcDomainParameters DomainParams { get; set; }
        public HashFunction HashAlg { get; set; }

        public ITestCaseExpectationProvider<SigFailureReasons> TestCaseExpectationProvider { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

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
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            L = expandoSource.GetTypeFromProperty<int>("l");
            N = expandoSource.GetTypeFromProperty<int>("n");

            var hashValue = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashValue))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashValue);
            }

            p = expandoSource.GetBigIntegerFromProperty("p");
            q = expandoSource.GetBigIntegerFromProperty("q");
            g = expandoSource.GetBigIntegerFromProperty("g");

            DomainParams = new FfcDomainParameters(p, q, g);

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
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

                case "hashalg":
                    var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(value);
                    HashAlg = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);
                    return true;
            }

            return false;
        }
    }
}
