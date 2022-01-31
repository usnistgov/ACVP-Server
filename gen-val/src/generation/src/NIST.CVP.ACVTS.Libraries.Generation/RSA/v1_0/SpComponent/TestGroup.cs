using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SpComponent
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public int Modulo { get; set; } = 2048;
        public PrivateKeyModes KeyFormat { get; set; }
        public string TestType { get; set; }

        // Both of these are implied by a property in the test cases
        [JsonIgnore]
        public PublicExponentModes PublicExponentMode { get; set; }

        [JsonIgnore]
        public BitString PublicExponent { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "mod":
                    Modulo = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
