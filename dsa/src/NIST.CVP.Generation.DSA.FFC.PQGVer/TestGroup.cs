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

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            L = (int)source.l;
            N = (int)source.n;

            var attributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm((string)source.hashAlg);
            HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize);

            if (((ExpandoObject)source).ContainsProperty("pqMode"))
            {
                PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(source.pqMode, false);
            }

            if (((ExpandoObject)source).ContainsProperty("gMode"))
            {
                GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(source.gMode, false);
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
            return ($"{L}{N}{HashAlg.Name}{EnumHelpers.GetEnumDescriptionFromEnum(PQGenMode)}{EnumHelpers.GetEnumDescriptionFromEnum(GGenMode)}").GetHashCode();
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
