using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
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
            var expandoSource = (ExpandoObject)source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            L = expandoSource.GetTypeFromProperty<int>("l");
            N = expandoSource.GetTypeFromProperty<int>("n");

            var hashValue = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashValue))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashValue);
            }

            PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(expandoSource.GetTypeFromProperty<string>("pqMode"), false);
            GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(expandoSource.GetTypeFromProperty<string>("gMode"), false);

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
