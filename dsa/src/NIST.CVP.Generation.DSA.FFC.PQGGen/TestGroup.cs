using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestGroup : ITestGroup
    {
        public GeneratorGenMode GGenMode { get; set; }
        public PrimeGenMode PQGenMode { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public HashFunction HashAlg { get; set; }

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

            var expandoSource = (ExpandoObject)source;

            L = expandoSource.GetTypeFromProperty<int>("l");
            N = expandoSource.GetTypeFromProperty<int>("n");

            var attributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(expandoSource.GetTypeFromProperty<string>("hashAlg"));
            HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize);

            PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(expandoSource.GetTypeFromProperty<string>("pqMode"), false);
            GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(expandoSource.GetTypeFromProperty<string>("gMode"), false);

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
            //var gMode = "";
            //if (GGenMode != GeneratorGenMode.None)
            //{
            //    gMode = EnumHelper.GGenModeToString(GGenMode);
            //}

            //var pqMode = "";
            //if (PQGenMode != PrimeGenMode.None)
            //{
            //    pqMode = EnumHelper.PQGenModeToString(PQGenMode);
            //}

            return ($"{L}{N}{HashAlg.Name}{EnumHelpers.GetEnumDescriptionFromEnum(GGenMode)}{EnumHelpers.GetEnumDescriptionFromEnum(PQGenMode)}").GetHashCode();
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
