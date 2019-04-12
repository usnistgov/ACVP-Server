using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ECDSA.v1_0.KeyVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }

        // Used internally to build test cases with particular error cases
        [JsonIgnore] public ITestCaseExpectationProvider<EcdsaKeyDisposition> TestCaseExpectationProvider { get; set; }

        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "curve":
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(value);
                    return true;
            }

            return false;
        }
    }
}
