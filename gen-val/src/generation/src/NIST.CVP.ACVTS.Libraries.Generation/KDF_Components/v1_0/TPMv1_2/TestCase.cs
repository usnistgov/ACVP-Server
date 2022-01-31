using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TPMv1_2
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }

        public BitString Auth { get; set; }
        public BitString NonceEven { get; set; }
        public BitString NonceOdd { get; set; }
        public BitString SKey { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "auth":
                    Auth = new BitString(value);
                    return true;

                case "nonce_even":
                    NonceEven = new BitString(value);
                    return true;

                case "nonce_odd":
                    NonceOdd = new BitString(value);
                    return true;

                case "skey":
                    SKey = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
