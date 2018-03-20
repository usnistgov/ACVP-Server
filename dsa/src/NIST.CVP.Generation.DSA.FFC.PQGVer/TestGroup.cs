using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Enums;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = (int) source.tgId;
            TestType = source.testType;
            
            L = (int)source.l;
            N = (int)source.n;

            var attributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm((string)source.hashAlg);
            HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize);

            if (expandoSource.ContainsProperty("pqMode"))
            {
                PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(source.pqMode, false);
            }

            if (expandoSource.ContainsProperty("gMode"))
            {
                GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(source.gMode, false);
            }

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public GeneratorGenMode GGenMode { get; set; }
        public PrimeGenMode PQGenMode { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public HashFunction HashAlg { get; set; }

        // Used internally to build test cases for the group
        public ITestCaseExpectationProvider<PQFailureReasons> PQTestCaseExpectationProvider { get; set; }
        public ITestCaseExpectationProvider<GFailureReasons> GTestCaseExpectationProvider { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "pqmode":
                    PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(value);
                    return true;

                case "gmode":
                    GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(value);
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;

                case "hashalg":
                    var attributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(value);
                    HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize);
                    return true;
            }

            return false;
        }
    }
}
