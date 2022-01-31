using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "gMode")]
        public GeneratorGenMode GGenMode { get; set; }
        [JsonProperty(PropertyName = "pqMode")]
        public PrimeGenMode PQGenMode { get; set; }
        [JsonProperty(PropertyName = "l")]
        public int L { get; set; }
        [JsonProperty(PropertyName = "n")]
        public int N { get; set; }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        // Used internally to build test cases for the group
        [JsonIgnore] public ITestCaseExpectationProvider<DsaPQDisposition> PQTestCaseExpectationProvider { get; set; }
        [JsonIgnore] public ITestCaseExpectationProvider<DsaGDisposition> GTestCaseExpectationProvider { get; set; }


        public List<TestCase> Tests { get; set; } = new List<TestCase>();

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
                    HashAlgName = value;
                    return true;
            }

            return false;
        }
    }
}
