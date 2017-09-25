using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestGroup : ITestGroup
    {
        public GeneratorGenMode GGenMode { get; set; }
        public PrimeGenMode PQGenMode { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public HashFunction HashAlg { get; set; }

        // This needs 2 copies of 'None'
        public List<PQFailureReasons> PQCovered = new List<PQFailureReasons>((PQFailureReasons[])Enum.GetValues(typeof(PQFailureReasons)));

        // This needs 3 copies of 'None' and 2 of 'ModifyG'
        public List<GFailureReasons> GCovered = new List<GFailureReasons>((GFailureReasons[])Enum.GetValues(typeof(GFailureReasons)));

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
                PQGenMode = EnumHelper.StringToPQGenMode(source.pqMode);
            }

            if (((ExpandoObject)source).ContainsProperty("gMode"))
            {
                GGenMode = EnumHelper.StringToGGenMode(source.gMode);
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
            return ($"{L}{N}{HashAlg.Name}{EnumHelper.GGenModeToString(GGenMode)}{EnumHelper.PQGenModeToString(PQGenMode)}").GetHashCode();
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
    }
}
